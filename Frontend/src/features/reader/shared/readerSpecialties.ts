export const READER_SPECIALTY_VALUES = ['love', 'career', 'general', 'health', 'finance'] as const;

export type ReaderSpecialtyValue = (typeof READER_SPECIALTY_VALUES)[number];

export function isReaderSpecialtyValue(value: string): value is ReaderSpecialtyValue {
 return READER_SPECIALTY_VALUES.includes(value as ReaderSpecialtyValue);
}
