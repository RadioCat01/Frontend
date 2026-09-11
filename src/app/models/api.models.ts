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
