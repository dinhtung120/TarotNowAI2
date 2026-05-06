'use client';

import { act } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import SystemConfigsListPanel from '@/features/admin/system-configs/SystemConfigsListPanel';
import type { AdminSystemConfigsViewModel } from '@/features/admin/system-configs/hooks/useAdminSystemConfigs';

describe('SystemConfigsListPanel', () => {
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

 it('prioritizes load-error state over empty-state rendering', () => {
  const vm = {
   t: (key: string) => ({
    'system_configs.filters.search_placeholder': 'SEARCH',
    'system_configs.meta.no_description': 'NO_DESCRIPTION',
    'system_configs.states.empty': 'EMPTY_STATE',
    'system_configs.states.retry': 'RETRY_LOAD',
   }[key] ?? key),
   searchText: '',
   setSearchText: vi.fn(),
   groupedItems: [],
   selectedKey: '',
   selectConfig: vi.fn(),
   filteredItems: [],
   hasLoadError: true,
   loadError: 'System config backend unavailable',
   retryLoadConfigs: vi.fn(),
   loading: false,
  } as unknown as AdminSystemConfigsViewModel;

  act(() => {
   root.render(<SystemConfigsListPanel vm={vm} />);
  });

  expect(container.textContent).toContain('System config backend unavailable');
  expect(container.textContent).not.toContain('EMPTY_STATE');
 });
});
