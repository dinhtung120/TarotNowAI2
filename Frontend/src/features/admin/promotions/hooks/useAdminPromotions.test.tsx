'use client';

import { act } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { useAdminPromotions } from '@/features/admin/promotions/hooks/useAdminPromotions';
import toast from 'react-hot-toast';
import {
 createPromotion,
 deletePromotion,
 listPromotions,
 updatePromotion,
} from '@/features/admin/promotions/actions';
import type { DepositPromotion } from '@/features/admin/promotions/actions';

vi.mock('next-intl', () => ({
 useLocale: vi.fn(() => 'vi'),
 useTranslations: vi.fn(() => (key: string) => key),
}));

vi.mock('react-hot-toast', () => ({
 default: {
  success: vi.fn(),
  error: vi.fn(),
 },
}));

vi.mock('@/features/admin/promotions/actions', () => ({
 listPromotions: vi.fn(),
 createPromotion: vi.fn(),
 updatePromotion: vi.fn(),
 deletePromotion: vi.fn(),
}));

const mockedListPromotions = vi.mocked(listPromotions);
const mockedCreatePromotion = vi.mocked(createPromotion);
const mockedUpdatePromotion = vi.mocked(updatePromotion);
const mockedDeletePromotion = vi.mocked(deletePromotion);
const mockedToast = vi.mocked(toast);

const PROMOTION_ITEM: DepositPromotion = {
 id: 'promo-1',
 minAmountVnd: 200_000,
 bonusGold: 500,
 isActive: true,
 createdAt: '2026-01-01T00:00:00.000Z',
};

function Harness({
 initialPromotions,
 initialLoadError,
 onChange,
}: {
 initialPromotions: DepositPromotion[];
 initialLoadError: string | null;
 onChange: (value: ReturnType<typeof useAdminPromotions>) => void;
}) {
 const value = useAdminPromotions(initialPromotions, initialLoadError);
 onChange(value);
 return null;
}

describe('useAdminPromotions', () => {
 let container: HTMLDivElement;
 let root: Root;

 beforeEach(() => {
  container = document.createElement('div');
  document.body.appendChild(container);
  root = createRoot(container);
 });

 afterEach(() => {
  act(() => {
   root.unmount();
  });
  container.remove();
  vi.clearAllMocks();
 });

 it('surfaces bootstrap load errors instead of collapsing to empty state', () => {
  let latest: ReturnType<typeof useAdminPromotions> | null = null;
  act(() => {
   root.render(<Harness initialPromotions={[]} initialLoadError="Failed to list promotions" onChange={(value) => {
    latest = value;
   }} />);
  });

  expect(latest?.promotions).toEqual([]);
  expect(latest?.listError).toBe('Failed to list promotions');
 });

 it('supports explicit retry flow and clears load error after a successful refresh', async () => {
  mockedListPromotions.mockResolvedValue({
   success: true,
   data: [PROMOTION_ITEM],
  } as never);

  let latest: ReturnType<typeof useAdminPromotions> | null = null;
  act(() => {
   root.render(<Harness initialPromotions={[]} initialLoadError="Failed to list promotions" onChange={(value) => {
    latest = value;
   }} />);
  });

 await act(async () => {
  await latest?.retryList();
 });

  expect(mockedListPromotions).toHaveBeenCalledWith(false);
  expect(latest?.listError).toBe('');
  expect(latest?.promotions).toEqual([PROMOTION_ITEM]);
 });

 it('creates promotion then refreshes list through explicit action flow', async () => {
  mockedCreatePromotion.mockResolvedValue({ success: true } as never);
  mockedListPromotions.mockResolvedValue({
   success: true,
   data: [PROMOTION_ITEM],
  } as never);

  let latest: ReturnType<typeof useAdminPromotions> | null = null;
  act(() => {
   root.render(<Harness initialPromotions={[]} initialLoadError={null} onChange={(value) => {
    latest = value;
   }} />);
  });

  act(() => {
   latest?.setIsCreating(true);
  });

  await act(async () => {
   await latest?.handleCreate({ minAmount: 200000, bonusGold: 500 });
  });

  expect(mockedCreatePromotion).toHaveBeenCalledWith(200000, 500);
  expect(mockedListPromotions).toHaveBeenCalledWith(false);
  expect(mockedToast.success).toHaveBeenCalledWith('promotions.toast.create_success');
  expect(latest?.isCreating).toBe(false);
 });

 it('toggles and deletes promotion entries with explicit success handling', async () => {
  mockedUpdatePromotion.mockResolvedValue({ success: true } as never);
  mockedDeletePromotion.mockResolvedValue({ success: true } as never);

  let latest: ReturnType<typeof useAdminPromotions> | null = null;
  act(() => {
   root.render(<Harness initialPromotions={[PROMOTION_ITEM]} initialLoadError={null} onChange={(value) => {
    latest = value;
   }} />);
  });

  await act(async () => {
   await latest?.handleToggle(PROMOTION_ITEM);
  });
  expect(mockedUpdatePromotion).toHaveBeenCalledWith('promo-1', {
   minAmountVnd: 200000,
   bonusGold: 500,
   isActive: false,
  });
  expect(latest?.promotions[0]?.isActive).toBe(false);

  act(() => {
   latest?.setDeleteId('promo-1');
  });
  await act(async () => {
   await latest?.handleDelete();
  });

  expect(mockedDeletePromotion).toHaveBeenCalledWith('promo-1');
  expect(latest?.promotions).toEqual([]);
  expect(mockedToast.success).toHaveBeenCalledWith('promotions.toast.delete_success');
 });
});
