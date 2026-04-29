import type { ReactElement } from 'react';
import { describe, expect, it, vi } from 'vitest';
import AdminPromotionsPage from '@/features/admin/promotions/presentation/AdminPromotionsPage';
import { listPromotions } from '@/features/admin/application/actions/promotion';

vi.mock('@/features/admin/application/actions/promotion', () => ({
 listPromotions: vi.fn(),
}));

vi.mock('@/features/admin/promotions/presentation/AdminPromotionsClient', () => ({
 default: vi.fn(() => null),
}));

const mockedListPromotions = vi.mocked(listPromotions);

describe('AdminPromotionsPage', () => {
 it('keeps bootstrap failure as explicit initialLoadError for client rendering', async () => {
  mockedListPromotions.mockResolvedValue({
   success: false,
   error: 'Failed to list promotions',
  } as never);

  const element = await AdminPromotionsPage() as ReactElement<{
   initialLoadError: string | null;
   initialPromotions: unknown[];
  }>;

  expect(element.props.initialPromotions).toEqual([]);
  expect(element.props.initialLoadError).toBe('Failed to list promotions');
 });
});
