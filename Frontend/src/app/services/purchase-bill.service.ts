import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PurchaseBillItem, PurchaseOrder, LatestPurchaseOrder, OldestPurchaseOrderItem, GroupedItem } from '../models/api.models';

@Injectable({ providedIn: 'root' })
export class PurchaseBillService {
  private readonly apiUrl = '/api/PurchaseBill';

  constructor(private readonly http: HttpClient) {}

  validateItem(item: Omit<PurchaseBillItem, 'totalCost' | 'totalSelling'>): Observable<PurchaseBillItem> {
    return this.http.post<PurchaseBillItem>(`${this.apiUrl}/items`, item);
  }

  savePurchaseOrder(items: PurchaseBillItem[]): Observable<{ message: string; id: number }> {
    return this.http.post<{ message: string; id: number }>(`${this.apiUrl}/save`, { items });
  }

  getLatestPurchaseOrders(): Observable<LatestPurchaseOrder[]> {
    return this.http.get<LatestPurchaseOrder[]>(`${this.apiUrl}/latest`);
  }

  getOldestPurchaseOrderItems(): Observable<OldestPurchaseOrderItem[]> {
    return this.http.get<OldestPurchaseOrderItem[]>(`${this.apiUrl}/oldest-items`);
  }

  getGroupedItems(): Observable<GroupedItem[]> {
    return this.http.get<GroupedItem[]>(`${this.apiUrl}/grouped-items`);
  }
}

