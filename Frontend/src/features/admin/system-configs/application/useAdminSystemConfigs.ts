'use client';

import { useMemo, useState } from 'react';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { useTranslations } from 'next-intl';
import toast from 'react-hot-toast';
import { listSystemConfigs, restartServer, updateSystemConfig } from '@/features/admin/application/actions';
import type { AdminSystemConfigItem } from '@/features/admin/system-configs/system-config.types';

const QUERY_KEY = ['admin', 'system-configs'] as const;

type SystemConfigDraft = {
 value: string;
 description: string;
 valueKind: 'scalar' | 'json';
};

function normalizeValueKind(valueKind: string): 'scalar' | 'json' {
 return valueKind.trim().toLowerCase() === 'json' ? 'json' : 'scalar';
}

function formatJsonValueForEditor(rawValue: string): string {
 try {
  return JSON.stringify(JSON.parse(rawValue), null, 2);
 } catch {
  return rawValue;
 }
}

function createDraft(item: AdminSystemConfigItem): SystemConfigDraft {
 const normalizedValueKind = normalizeValueKind(item.valueKind);
 return {
  value: normalizedValueKind === 'json' ? formatJsonValueForEditor(item.value) : item.value,
  description: item.description ?? '',
  valueKind: normalizedValueKind,
 };
}

function emptyDraft(): SystemConfigDraft {
 return {
  value: '',
  description: '',
  valueKind: 'scalar',
 };
}

function resolveErrorMessage(error: unknown, fallback: string): string {
 if (error instanceof Error && error.message.trim().length > 0) {
  return error.message;
 }

 return fallback;
}

export function useAdminSystemConfigs(initialConfigs: AdminSystemConfigItem[]) {
 const t = useTranslations('Admin');
 const queryClient = useQueryClient();
 const [selectedKey, setSelectedKey] = useState(initialConfigs[0]?.key ?? '');
 const [searchText, setSearchText] = useState('');
 const [drafts, setDrafts] = useState<Record<string, SystemConfigDraft>>({});

 const query = useQuery({
  queryKey: QUERY_KEY,
  queryFn: async () => {
   const result = await listSystemConfigs();
   if (!result.success) {
    throw new Error(result.error || t('system_configs.states.load_error'));
   }

   return result.data ?? [];
  },
  initialData: initialConfigs,
 });

 const items = useMemo(() => query.data ?? [], [query.data]);

 const selectedItem = useMemo(() => {
  if (items.length === 0) {
   return null;
  }

  return items.find((item) => item.key === selectedKey) ?? items[0];
 }, [items, selectedKey]);

 const activeDraft = useMemo(() => {
  if (!selectedItem) {
   return emptyDraft();
  }

  return drafts[selectedItem.key] ?? createDraft(selectedItem);
 }, [drafts, selectedItem]);

 const filteredItems = useMemo(() => {
  const keyword = searchText.trim().toLowerCase();
  if (!keyword) {
   return items;
  }

  return items.filter((item) => {
   const description = item.description?.toLowerCase() ?? '';
   return item.key.toLowerCase().includes(keyword) || description.includes(keyword);
  });
 }, [items, searchText]);

 const groupedItems = useMemo(() => {
  const groups = new Map<string, AdminSystemConfigItem[]>();
  for (const item of filteredItems) {
   const prefix = item.key.split('.')[0] || 'other';
   const groupName = prefix.toLowerCase();
   const existing = groups.get(groupName) || [];
   existing.push(item);
   groups.set(groupName, existing);
  }
  
  return Array.from(groups.entries())
   .sort((a, b) => a[0].localeCompare(b[0]))
   .map(([groupName, items]) => ({
    groupName,
    items: items.sort((a, b) => a.key.localeCompare(b.key)),
   }));
 }, [filteredItems]);

 const loadError = query.isError
  ? resolveErrorMessage(query.error, t('system_configs.states.load_error'))
  : '';
 const hasLoadError = !query.isFetching && loadError.trim().length > 0;

 const updateMutation = useMutation({
  mutationFn: async () => {
   if (!selectedItem) {
    throw new Error('No selected config item.');
   }

   return updateSystemConfig(selectedItem.key, {
    value: activeDraft.value,
    valueKind: activeDraft.valueKind,
    description: activeDraft.description,
   });
  },
 });

 const restartMutation = useMutation({
  mutationFn: async () => {
   return restartServer();
  },
 });

 function updateSelectedDraft(updater: (current: SystemConfigDraft) => SystemConfigDraft) {
  if (!selectedItem) {
   return;
  }

  setDrafts((current) => {
   const baseDraft = current[selectedItem.key] ?? createDraft(selectedItem);
   return {
    ...current,
    [selectedItem.key]: updater(baseDraft),
   };
  });
 }

 async function saveSelectedConfig() {
  if (!selectedItem) {
   toast.error(t('system_configs.toast.select_item'));
   return;
  }

  const result = await updateMutation.mutateAsync();
  if (!result.success || !result.data) {
   toast.error(result.error || t('system_configs.toast.save_failed'));
   return;
  }

  const updated = result.data;
  setDrafts((current) => ({
   ...current,
   [updated.key]: createDraft(updated),
  }));

  queryClient.setQueryData<AdminSystemConfigItem[]>(QUERY_KEY, (current) =>
   (current ?? []).map((item) => (item.key === updated.key ? updated : item)),
  );
  await queryClient.invalidateQueries({ queryKey: QUERY_KEY });
  toast.success(t('system_configs.toast.save_success'));
 }

 async function handleRestartServer() {
  const result = await restartMutation.mutateAsync();
  if (!result.success) {
   toast.error(result.error || t('system_configs.toast.restart_failed') || 'Failed to create restart request');
   return;
  }
  toast.success(t('system_configs.toast.restart_success') || 'Restart request submitted');
 }

 function selectConfig(item: AdminSystemConfigItem) {
  setSelectedKey(item.key);
 }

 async function retryLoadConfigs() {
  await query.refetch();
 }

 return {
  t,
  items,
  filteredItems,
  selectedKey: selectedItem?.key ?? '',
  selectedItem,
  searchText,
  draftValue: activeDraft.value,
  draftDescription: activeDraft.description,
  draftValueKind: activeDraft.valueKind,
  loading: query.isLoading || query.isFetching,
  loadError,
  hasLoadError,
  saving: updateMutation.isPending,
  setSearchText,
  setDraftValue: (value: string) => updateSelectedDraft((current) => ({ ...current, value })),
  setDraftDescription: (description: string) => updateSelectedDraft((current) => ({ ...current, description })),
  setDraftValueKind: (valueKind: 'scalar' | 'json') =>
   updateSelectedDraft((current) => ({
    ...current,
    valueKind,
   })),
  selectConfig,
  retryLoadConfigs,
  saveSelectedConfig,
  handleRestartServer,
  restarting: restartMutation.isPending,
  groupedItems,
 };
}

export type AdminSystemConfigsViewModel = ReturnType<typeof useAdminSystemConfigs>;
