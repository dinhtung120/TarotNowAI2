export interface AdminSystemConfigItem {
 key: string;
 value: string;
 valueKind: string;
 description?: string | null;
 updatedBy?: string | null;
 updatedAt?: string | null;
 isKnownKey: boolean;
 source: string;
}

export interface UpdateSystemConfigParams {
 value: string;
 valueKind?: string;
 description?: string;
}
