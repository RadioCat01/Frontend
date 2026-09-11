import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PurchaseBillItem } from '../models/api.models';

@Injectable({ providedIn: 'root' })
export class PurchaseBillService {
  private readonly apiUrl = '/api/PurchaseBill';

  constructor(private readonly http: HttpClient) {}

  validateItem(item: Omit<PurchaseBillItem, 'totalCost' | 'totalSelling'>): Observable<PurchaseBillItem> {
    return this.http.post<PurchaseBillItem>(`${this.apiUrl}/items`, item);
  }
}
