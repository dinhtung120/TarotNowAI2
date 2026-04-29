'use client';

import type { PayoutBankOption, ProfileDto } from '@/features/profile/application/actions';
import type { ProfileFormValues } from '@/features/profile/application/useProfilePage.types';

export function normalizeOptional(value: string): string | null {
 const normalized = value.trim();
 return normalized ? normalized : null;
}

export function resolveBankName(options: PayoutBankOption[], bankBin: string): string | null {
 const normalizedBankBin = bankBin.trim();
 if (!normalizedBankBin) {
  return null;
 }

 return options.find((item) => item.bankBin === normalizedBankBin)?.bankName ?? null;
}

export function toDateInputValue(rawDate: string): string | null {
 const dateOnlyMatch = /^(\d{4})-(\d{2})-(\d{2})/.exec(rawDate.trim());
 if (dateOnlyMatch) {
  return `${dateOnlyMatch[1]}-${dateOnlyMatch[2]}-${dateOnlyMatch[3]}`;
 }

 const parsed = new Date(rawDate);
 if (Number.isNaN(parsed.getTime())) {
  return null;
 }

 const yyyy = parsed.getUTCFullYear();
 const mm = String(parsed.getUTCMonth() + 1).padStart(2, '0');
 const dd = String(parsed.getUTCDate()).padStart(2, '0');
 return `${yyyy}-${mm}-${dd}`;
}

export function buildProfileFormValues(
 profile: ProfileDto | null | undefined,
 profileError: string | null | undefined,
): ProfileFormValues | null {
 if (!profile || profileError) {
  return null;
 }

 return {
  displayName: profile.displayName,
  payoutBankBin: profile.payoutBankBin ?? '',
  payoutBankAccountNumber: profile.payoutBankAccountNumber ?? '',
  payoutBankAccountHolder: profile.payoutBankAccountHolder ?? '',
  dateOfBirth: toDateInputValue(profile.dateOfBirth) ?? '',
 };
}

export function revokeObjectUrlIfNeeded(value: string | null) {
 if (!value || !value.startsWith('blob:')) {
  return;
 }

 URL.revokeObjectURL(value);
}
