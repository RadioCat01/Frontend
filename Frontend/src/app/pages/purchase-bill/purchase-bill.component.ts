import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';
import { AuthService } from '../../services/auth.service';
import { PurchaseBillService } from '../../services/purchase-bill.service';
import { Location, PurchaseBillItem } from '../../models/api.models';

@Component({
  selector: 'app-purchase-bill',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './purchase-bill.component.html',
  styleUrl: './purchase-bill.component.scss'
})
export class PurchaseBillComponent implements OnInit {
  readonly now = new Date();
  readonly itemNames = ['Mango', 'Apple', 'Banana', 'Orange', 'Grapes', 'Kiwi', 'Strawberry'];
  readonly itemForm;
  locations: Location[] = [];
  items: PurchaseBillItem[] = [];
  isLoading = true;
  isAdding = false;
  isSaving = false;
  errorMessage = '';
  successMessage = '';

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly authService: AuthService,
    private readonly purchaseBillService: PurchaseBillService,
    private readonly router: Router
  ) {
    this.itemForm = this.formBuilder.nonNullable.group({
      item: ['', Validators.required],
      batch: ['', Validators.required],
      standardCost: [0, [Validators.required, Validators.min(0)]],
      standardPrice: [0, [Validators.required, Validators.min(0)]],
      qty: [1, [Validators.required, Validators.min(1)]],
      discountPercent: [0, [Validators.required, Validators.min(0), Validators.max(100)]]
    });
  }

  ngOnInit(): void {
    this.authService.getLocations().pipe(finalize(() => this.isLoading = false)).subscribe({
      next: (locations) => this.locations = locations,
      error: () => {
        this.locations = this.authService.getStoredLocations();
        if (!this.locations.length) this.errorMessage = 'Locations could not be loaded. Please sign in again.';
      }
    });
  }

  get previewTotalCost(): number {
    const value = this.itemForm.getRawValue();
    return (value.standardCost * value.qty) * (1 - value.discountPercent / 100);
  }

  get previewTotalSelling(): number {
    const value = this.itemForm.getRawValue();
    return value.standardPrice * value.qty;
  }

  get totalQuantity(): number {
    return this.items.reduce((total, item) => total + item.qty, 0);
  }

  get itemsTotal(): number {
    return this.items.reduce((total, item) => total + item.totalCost, 0);
  }

  addItem(): void {
    this.errorMessage = '';
    this.successMessage = '';
    if (this.itemForm.invalid) {
      this.itemForm.markAllAsTouched();
      return;
    }

    this.isAdding = true;
    this.purchaseBillService.validateItem(this.itemForm.getRawValue()).pipe(
      finalize(() => this.isAdding = false)
    ).subscribe({
      next: (item) => {
        this.items = [...this.items, item];
        this.successMessage = `${item.item} was added to the bill.`;
        this.itemForm.reset({ item: '', batch: '', standardCost: 0, standardPrice: 0, qty: 1, discountPercent: 0 });
      },
      error: (error) => this.errorMessage = error.error?.message ?? 'The item could not be added.'
    });
  }

  removeItem(index: number): void {
    this.items = this.items.filter((_, itemIndex) => itemIndex !== index);
  }

  savePurchaseOrder(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (this.items.length === 0) {
      this.errorMessage = 'Please add at least one item before saving.';
      return;
    }

    this.isSaving = true;
    this.purchaseBillService.savePurchaseOrder(this.items).pipe(
      finalize(() => this.isSaving = false)
    ).subscribe({
      next: (response) => {
        this.successMessage = `Purchase order #${response.id} saved successfully!`;
        this.items = [];
        this.itemForm.reset({ item: '', batch: '', standardCost: 0, standardPrice: 0, qty: 1, discountPercent: 0 });
      },
      error: (error) => this.errorMessage = error.error?.message ?? 'Failed to save purchase order.'
    });
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
