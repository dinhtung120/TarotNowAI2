import { AlertTriangle, RefreshCcw } from 'lucide-react';
import { cn } from '@/lib/utils';
import { Button } from '@/shared/ui';
import type { AdminSystemConfigsViewModel } from '@/features/admin/system-configs/hooks/useAdminSystemConfigs';

interface SystemConfigsLoadErrorStateProps {
 vm: AdminSystemConfigsViewModel;
}

export default function SystemConfigsLoadErrorState({ vm }: SystemConfigsLoadErrorStateProps) {
 return (
  <div className={cn('flex flex-col items-center justify-center gap-4 py-20 text-center')}>
   <div className={cn('flex h-16 w-16 items-center justify-center rounded-full border border-red-400/40 bg-red-500/10')}>
    <AlertTriangle className={cn('h-8 w-8 text-red-300')} />
   </div>
   <p className={cn('max-w-md text-sm font-semibold text-red-100')}>{vm.loadError}</p>
   <Button variant="secondary" onClick={() => {
    void vm.retryLoadConfigs();
   }}>
    <span className={cn('inline-flex items-center gap-2')}>
     <RefreshCcw className={cn('h-4 w-4')} />
     {vm.t('system_configs.states.retry')}
    </span>
   </Button>
  </div>
 );
}
