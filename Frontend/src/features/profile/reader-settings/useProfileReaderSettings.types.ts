import type { ReaderStatus } from '@/features/reader/shared/readerStatus';
import type { ReaderSpecialtyValue } from '@/features/reader/shared/readerSpecialties';

export type TranslateFn = (key: string, values?: Record<string, string | number | Date>) => string;

export interface ReaderSettingsDraft {
 bioVi: string;
 diamondPerQuestion: number;
 specialties: ReaderSpecialtyValue[];
 yearsOfExperience: number;
 facebookUrl: string;
 instagramUrl: string;
 tikTokUrl: string;
 status: ReaderStatus;
}

export interface ReaderSettingsSubmitPayload {
 bio: string;
 specialties: ReaderSpecialtyValue[];
 years: number;
 price: number;
 facebookUrl: string;
 instagramUrl: string;
 tikTokUrl: string;
}
