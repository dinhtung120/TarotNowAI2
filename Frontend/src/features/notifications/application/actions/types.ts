export interface NotificationItem {
 id: string;
 userId: string;
 titleVi: string;
 titleEn: string;
 bodyVi: string;
 bodyEn: string;
 type: string;
 isRead: boolean;
 createdAt: string;
}

export interface NotificationListResponse {
 items: NotificationItem[];
 totalCount: number;
 page: number;
 pageSize: number;
}
