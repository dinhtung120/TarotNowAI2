import type { ReaderProfile } from '@/features/reader/shared';

export interface ReaderDirectoryStatusLabels {
 online: string;
 busy: string;
 offline: string;
}

export interface ReaderDirectoryCardLabels extends ReaderDirectoryStatusLabels {
 readerFallback: string;
 perQuestionSuffix: string;
 yearsExperienceLabel: string;
 socialLinksLabel: string;
}

export interface ReaderDirectoryListLabels extends ReaderDirectoryCardLabels {
 loading: string;
 empty: string;
}

export interface ReaderDirectoryFilterOption {
 value: string;
 label: string;
}

export type ReaderBioResolver = (reader: ReaderProfile) => string;
