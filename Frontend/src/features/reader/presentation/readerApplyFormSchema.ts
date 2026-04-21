import { z } from 'zod';
import type { MyReaderRequest } from '@/features/reader/application/actions';
import {
 isReaderSpecialtyValue,
 READER_SPECIALTY_VALUES,
 type ReaderSpecialtyValue,
} from '@/features/reader/domain/readerSpecialties';
import {
 hasAtLeastOneSocialLink,
 isValidFacebookUrl,
 isValidInstagramUrl,
 isValidTikTokUrl,
 normalizeOptionalSocialUrl,
} from '@/features/reader/domain/readerSocialLinks';

type TranslateFn = (key: string, values?: Record<string, string | number | Date>) => string;

const specialtySchema = z.enum(READER_SPECIALTY_VALUES);
const DEFAULT_DIAMOND_PER_QUESTION = 100;
const MAX_BIO_LENGTH = 4_000;
const MAX_SOCIAL_URL_LENGTH = 500;

export function createReaderApplyFormSchema(t: TranslateFn) {
 return z.object({
  bio: z.string().trim().min(20, t('validation.bio_min')).max(MAX_BIO_LENGTH, t('validation.bio_max')),
  specialties: z.array(specialtySchema).min(1, t('validation.specialties_min')),
  yearsOfExperience: z.number().int().min(1, t('validation.years_min')),
  diamondPerQuestion: z.number().int().min(50, t('validation.diamond_min')),
  facebookUrl: z.string().trim().max(MAX_SOCIAL_URL_LENGTH, t('validation.social_too_long')),
  instagramUrl: z.string().trim().max(MAX_SOCIAL_URL_LENGTH, t('validation.social_too_long')),
  tikTokUrl: z.string().trim().max(MAX_SOCIAL_URL_LENGTH, t('validation.social_too_long')),
 }).superRefine((values, context) => {
  if (!hasAtLeastOneSocialLink(values)) {
   context.addIssue({
    code: z.ZodIssueCode.custom,
    path: ['facebookUrl'],
    message: t('validation.social_required'),
   });
  }

  if (!isValidFacebookUrl(values.facebookUrl)) {
   context.addIssue({
    code: z.ZodIssueCode.custom,
    path: ['facebookUrl'],
    message: t('validation.facebook_invalid'),
   });
  }

  if (!isValidInstagramUrl(values.instagramUrl)) {
   context.addIssue({
    code: z.ZodIssueCode.custom,
    path: ['instagramUrl'],
    message: t('validation.instagram_invalid'),
   });
  }

  if (!isValidTikTokUrl(values.tikTokUrl)) {
   context.addIssue({
    code: z.ZodIssueCode.custom,
    path: ['tikTokUrl'],
    message: t('validation.tiktok_invalid'),
   });
  }
 });
}

export type ReaderApplyFormValues = z.infer<ReturnType<typeof createReaderApplyFormSchema>>;

export function mapReaderRequestToFormValues(
 existingRequest: MyReaderRequest | null | undefined,
): ReaderApplyFormValues {
 return {
  bio: existingRequest?.bio ?? '',
  specialties: normalizeSpecialties(existingRequest?.specialties),
  yearsOfExperience: Math.max(1, existingRequest?.yearsOfExperience ?? 1),
  diamondPerQuestion: Math.max(50, existingRequest?.diamondPerQuestion ?? DEFAULT_DIAMOND_PER_QUESTION),
  facebookUrl: normalizeOptionalSocialUrl(existingRequest?.facebookUrl),
  instagramUrl: normalizeOptionalSocialUrl(existingRequest?.instagramUrl),
  tikTokUrl: normalizeOptionalSocialUrl(existingRequest?.tikTokUrl),
 };
}

function normalizeSpecialties(values: string[] | undefined): ReaderSpecialtyValue[] {
 if (!values || values.length === 0) {
  return [];
 }

 return values.filter(isReaderSpecialtyValue);
}
