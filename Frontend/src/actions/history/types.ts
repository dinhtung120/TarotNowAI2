export interface AdminHistorySessionItem {
 id: string;
 userId: string;
 username: string;
 spreadType: string;
 question: string | null;
 isCompleted: boolean;
 createdAt: string;
}

export interface AdminHistoryPaginatedResponse {
 page: number;
 pageSize: number;
 totalPages: number;
 totalCount: number;
 items: AdminHistorySessionItem[];
}
