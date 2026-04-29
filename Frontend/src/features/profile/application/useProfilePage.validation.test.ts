import { describe, expect, it } from 'vitest';
import { createProfileSchema } from '@/features/profile/application/useProfilePage.validation';

const t = (key: string) => key;

describe('createProfileSchema', () => {
 it('accepts non-reader profiles without payout fields', () => {
  const schema = createProfileSchema({ isTarotReader: false, t });
  const result = schema.safeParse({
   displayName: 'User One',
   dateOfBirth: '2000-01-01',
   payoutBankBin: '',
   payoutBankAccountNumber: '',
   payoutBankAccountHolder: '',
  });

  expect(result.success).toBe(true);
 });

 it('requires full payout information when tarot reader fills any payout field', () => {
  const schema = createProfileSchema({ isTarotReader: true, t });
  const result = schema.safeParse({
   displayName: 'Reader One',
   dateOfBirth: '2000-01-01',
   payoutBankBin: '',
   payoutBankAccountNumber: '',
   payoutBankAccountHolder: '',
  });

  expect(result.success).toBe(true);

  const invalidResult = schema.safeParse({
   displayName: 'Reader One',
   dateOfBirth: '2000-01-01',
   payoutBankBin: '',
   payoutBankAccountNumber: '12345',
   payoutBankAccountHolder: 'Reader',
  });

  expect(invalidResult.success).toBe(false);
  const messages = invalidResult.success ? [] : invalidResult.error.issues.map((issue) => issue.message);
  expect(messages).toContain('validation.bank_required');
  expect(messages).toContain('validation.account_number_invalid');
  expect(messages).toContain('validation.account_holder_invalid');
 });
});
