export interface Location {
  locationCode: string;
  locationName: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  success: boolean;
  token: string;
  message: string;
  locations: Location[];
}

export interface PurchaseBillItem {
  item: string;
  batch: string;
  standardCost: number;
  standardPrice: number;
  qty: number;
  discountPercent: number;
  totalCost: number;
  totalSelling: number;
}

export interface PurchaseOrder {
  id: number;
  createdDate: Date;
  totalCost: number;
  totalSelling: number;
  totalItems: number;
  items: PurchaseBillItem[];
}

export interface LatestPurchaseOrder {
  id: number;
  netAmount: number;
  noOfItems: number;
}

export interface OldestPurchaseOrderItem {
  purchaseOrderId: number;
  itemName: string;
  noOfQuantity: number;
}

export interface GroupedItem {
  itemName: string;
  totalQuantity: number;
}

