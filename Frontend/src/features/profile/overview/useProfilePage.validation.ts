'use client';

import * as z from 'zod';

interface CreateProfileSchemaParams {
 isTarotReader: boolean;
 t: (key: string) => string;
}

const MIN_ACCOUNT_NUMBER_LENGTH = 6;
const MAX_ACCOUNT_NUMBER_LENGTH = 32;

function isUppercaseNoAccent(value: string): boolean {
 const normalized = value.trim();
 if (!normalized || normalized !== normalized.toUpperCase()) {
  return false;
 }

 if (/[\u0300-\u036f]/.test(normalized.normalize('NFD'))) {
  return false;
 }

 return /^[A-Z ]+$/.test(normalized);
}

export function createProfileSchema({ isTarotReader, t }: CreateProfileSchemaParams) {
 return z.object({
  displayName: z.string().min(2, t('validation.display_name_min')),
  dateOfBirth: z.string().refine((date) => !Number.isNaN(Date.parse(date)), {
   message: t('validation.date_of_birth_invalid'),
  }),
  payoutBankBin: z.string(),
  payoutBankAccountNumber: z.string().max(MAX_ACCOUNT_NUMBER_LENGTH, t('validation.account_number_invalid')),
  payoutBankAccountHolder: z.string().max(120, t('validation.account_holder_invalid')),
 }).superRefine((value, context) => {
  if (!isTarotReader) {
   return;
  }

  const hasAnyPayoutField = Boolean(
   value.payoutBankBin.trim()
   || value.payoutBankAccountNumber.trim()
   || value.payoutBankAccountHolder.trim(),
  );

  if (!hasAnyPayoutField) {
   return;
  }

  if (!value.payoutBankBin.trim()) {
   context.addIssue({
    code: z.ZodIssueCode.custom,
    message: t('validation.bank_required'),
    path: ['payoutBankBin'],
   });
  }

  const accountNumber = value.payoutBankAccountNumber.trim();
  if (!accountNumber) {
   context.addIssue({
    code: z.ZodIssueCode.custom,
    message: t('validation.account_number_required'),
    path: ['payoutBankAccountNumber'],
   });
  } else if (!/^\d+$/.test(accountNumber)
   || accountNumber.length < MIN_ACCOUNT_NUMBER_LENGTH
   || accountNumber.length > MAX_ACCOUNT_NUMBER_LENGTH) {
   context.addIssue({
    code: z.ZodIssueCode.custom,
    message: t('validation.account_number_invalid'),
    path: ['payoutBankAccountNumber'],
   });
  }

  const accountHolder = value.payoutBankAccountHolder.trim();
  if (!accountHolder) {
   context.addIssue({
    code: z.ZodIssueCode.custom,
    message: t('validation.account_holder_required'),
    path: ['payoutBankAccountHolder'],
   });
  } else if (!isUppercaseNoAccent(accountHolder)) {
   context.addIssue({
    code: z.ZodIssueCode.custom,
    message: t('validation.account_holder_invalid'),
    path: ['payoutBankAccountHolder'],
   });
  }
 });
}
