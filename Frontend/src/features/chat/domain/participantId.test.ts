import { describe, expect, it } from 'vitest';
import { isSameParticipantId, normalizeParticipantId } from '@/features/chat/domain/participantId';

describe('participantId', () => {
  it('normalizes id with trim + lowercase', () => {
    expect(normalizeParticipantId('  ABCD-EFGH  ')).toBe('abcd-efgh');
  });

  it('compares ids case-insensitively', () => {
    expect(
      isSameParticipantId(
        '11111111-1111-1111-1111-111111111111',
        '11111111-1111-1111-1111-111111111111'.toUpperCase(),
      ),
    ).toBe(true);
  });

  it('returns false when either side is empty', () => {
    expect(isSameParticipantId('', '  ')).toBe(false);
    expect(isSameParticipantId(undefined, '123')).toBe(false);
  });
});
