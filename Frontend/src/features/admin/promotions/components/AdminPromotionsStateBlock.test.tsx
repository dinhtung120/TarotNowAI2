'use client';

import { act } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { AdminPromotionsStateBlock } from '@/features/admin/promotions/components/AdminPromotionsStateBlock';

vi.mock('next-intl', () => ({
 useTranslations: vi.fn(() => (key: string) => ({
  'promotions.states.loading': 'LOADING_PROMOTIONS',
  'promotions.states.empty': 'EMPTY_PROMOTIONS',
  'promotions.states.retry': 'RETRY_PROMOTIONS',
 }[key] ?? key)),
}));

describe('AdminPromotionsStateBlock', () => {
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

 it('renders explicit error state and avoids empty placeholder when list load fails', () => {
  const onRetry = vi.fn();
  act(() => {
   root.render(<AdminPromotionsStateBlock loading={false} error="Failed to list promotions" onRetry={onRetry} />);
  });

  expect(container.textContent).toContain('Failed to list promotions');
  expect(container.textContent).not.toContain('EMPTY_PROMOTIONS');
 });
});
