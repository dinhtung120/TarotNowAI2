'use client';

import { useState } from 'react';
import { Power, Settings } from 'lucide-react';
import { cn } from '@/lib/utils';
import { useAdminSystemConfigs } from '@/features/admin/system-configs/hooks/useAdminSystemConfigs';
import type { AdminSystemConfigItem } from '@/features/admin/system-configs/system-config.types';
import { Button, SectionHeader } from '@/shared/ui';
import ActionConfirmModal from '@/shared/ui/ActionConfirmModal';
import SystemConfigsEditorPanel from '@/features/admin/system-configs/SystemConfigsEditorPanel';
import SystemConfigsListPanel from '@/features/admin/system-configs/SystemConfigsListPanel';

interface AdminSystemConfigsPageProps {
 initialConfigs: AdminSystemConfigItem[];
}

function formatUpdatedAt(rawValue?: string | null): string {
 if (!rawValue) {
  return '';
 }

 const parsed = new Date(rawValue);
 return Number.isNaN(parsed.getTime()) ? '' : parsed.toLocaleString();
}

export default function AdminSystemConfigsPage({ initialConfigs }: AdminSystemConfigsPageProps) {
 const vm = useAdminSystemConfigs(initialConfigs);
 const selectedUpdatedAt = formatUpdatedAt(vm.selectedItem?.updatedAt);
 const [confirmRestartOpen, setConfirmRestartOpen] = useState(false);

 const handleConfirmRestart = async () => {
  await vm.handleRestartServer();
  setConfirmRestartOpen(false);
 };

 return (
  <div className={cn('animate-in space-y-8 pb-20 duration-700 fade-in')}>
   <SectionHeader
    tag={vm.t('system_configs.header.tag')}
    tagIcon={<Settings className={cn('h-3 w-3 tn-text-accent')} />}
    title={vm.t('system_configs.header.title')}
    subtitle={vm.t('system_configs.header.subtitle', { count: vm.items.length })}
    className={cn('items-start text-left')}
    action={
     <Button
      variant="danger"
      disabled={vm.restarting}
      onClick={() => setConfirmRestartOpen(true)}
      className={cn('whitespace-nowrap px-5 py-3 tn-shadow-accent-20')}
     >
      <span className={cn('inline-flex items-center gap-2')}>
       <Power className={cn('h-4 w-4')} />
       {vm.restarting
        ? vm.t('system_configs.actions.restarting') || 'Restarting...'
        : vm.t('system_configs.actions.restart') || 'Restart Server'}
      </span>
     </Button>
    }
   />

   <div className={cn('grid gap-6 xl:grid-cols-[380px_minmax(0,1fr)]')}>
   <SystemConfigsListPanel vm={vm} />
   <SystemConfigsEditorPanel vm={vm} selectedUpdatedAt={selectedUpdatedAt} />
   </div>
   <ActionConfirmModal
    open={confirmRestartOpen}
    icon={<Power className={cn('h-6 w-6 text-red-400')} />}
    title={vm.t('system_configs.actions.restart') || 'Restart Server'}
    description={vm.t('system_configs.toast.restart_confirm') || 'Yêu cầu restart sẽ được gửi để duyệt. Tiếp tục?'}
    confirmLabel={vm.t('system_configs.actions.restart') || 'Restart'}
    cancelLabel={vm.t('common.cancel') || 'Cancel'}
    confirmVariant="danger"
    confirmLoading={vm.restarting}
    onCancel={() => setConfirmRestartOpen(false)}
    onConfirm={() => {
     void handleConfirmRestart();
    }}
   />
  </div>
 );
}
