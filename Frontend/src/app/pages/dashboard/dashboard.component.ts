import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subject, interval, takeUntil, switchMap } from 'rxjs';
import { AuthService } from '../../services/auth.service';
import { PurchaseBillService } from '../../services/purchase-bill.service';
import { LatestPurchaseOrder, OldestPurchaseOrderItem, GroupedItem } from '../../models/api.models';
import { NgxChartsModule } from '@swimlane/ngx-charts';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, NgxChartsModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit, OnDestroy {
  latestOrders: LatestPurchaseOrder[] = [];
  oldestItems: OldestPurchaseOrderItem[] = [];
  groupedItems: GroupedItem[] = [];

  isLoadingLatest = true;
  isLoadingOldest = true;
  isLoadingGrouped = true;

  errorMessage = '';

  donutChartData: any[] = [];
  chartView: [number, number] = [300, 300];

  private destroy$ = new Subject<void>();

  constructor(
    private readonly purchaseBillService: PurchaseBillService,
    private readonly authService: AuthService,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    this.loadDashboardData();

    // Refresh data every 30 seconds
    interval(30000)
      .pipe(
        switchMap(() => {
          this.loadDashboardData();
          return [];
        }),
        takeUntil(this.destroy$)
      )
      .subscribe();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private loadDashboardData(): void {
    this.loadLatestOrders();
    this.loadOldestItems();
    this.loadGroupedItems();
  }

  private loadLatestOrders(): void {
    this.isLoadingLatest = true;
    this.purchaseBillService.getLatestPurchaseOrders().subscribe({
      next: (orders) => {
        this.latestOrders = orders;
        this.isLoadingLatest = false;
      },
      error: (error) => {
        console.error('Error loading latest orders:', error);
        this.isLoadingLatest = false;
      }
    });
  }

  private loadOldestItems(): void {
    this.isLoadingOldest = true;
    this.purchaseBillService.getOldestPurchaseOrderItems().subscribe({
      next: (items) => {
        this.oldestItems = items;
        this.isLoadingOldest = false;
      },
      error: (error) => {
        console.error('Error loading oldest items:', error);
        this.isLoadingOldest = false;
      }
    });
  }

  private loadGroupedItems(): void {
    this.isLoadingGrouped = true;
    this.purchaseBillService.getGroupedItems().subscribe({
      next: (items) => {
        this.groupedItems = items;
        this.donutChartData = items.map(item => ({
          name: item.itemName,
          value: item.totalQuantity
        }));
        this.isLoadingGrouped = false;
      },
      error: (error) => {
        console.error('Error loading grouped items:', error);
        this.isLoadingGrouped = false;
      }
    });
  }

  navigateToPurchaseBill(): void {
    this.router.navigate(['/purchase-bill']);
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  getTotalQuantity(): number {
    return this.groupedItems.reduce((total, item) => total + item.totalQuantity, 0);
  }
}
