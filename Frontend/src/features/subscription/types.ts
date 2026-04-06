export interface SubscriptionPlan {
  id: string;
  name: string;
  description: string;
  priceDiamond: number;
  durationDays: number;
  isActive: boolean;
  entitlements: Record<string, number>;
  createdAt: string;
}

export interface EntitlementBalance {
  entitlementKey: string;
  dailyQuotaTotal: number;
  usedToday: number;
  remainingToday: number;
}

export interface SubscribeRequest {
  planId: string;
  idempotencyKey: string;
}

export interface SubscribeResponse {
  subscriptionId: string;
}
