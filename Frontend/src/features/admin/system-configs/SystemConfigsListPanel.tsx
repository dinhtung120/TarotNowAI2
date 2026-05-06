import { AlertTriangle, RefreshCcw, Search } from 'lucide-react';
import { cn } from '@/lib/utils';
import { Badge, Button, GlassCard, Input } from '@/shared/ui';
import type { AdminSystemConfigsViewModel } from '@/features/admin/system-configs/hooks/useAdminSystemConfigs';

interface SystemConfigsListPanelProps {
 vm: AdminSystemConfigsViewModel;
}

export default function SystemConfigsListPanel({ vm }: SystemConfigsListPanelProps) {
 return (
  <GlassCard padding="sm" className={cn('flex flex-col gap-4 tn-panel shadow-[var(--shadow-card)]')}>
   <div className={cn('px-2 pt-2')}>
    <Input
     leftIcon={<Search className={cn('h-4 w-4')} />}
     value={vm.searchText}
     onChange={(event) => vm.setSearchText(event.target.value)}
     placeholder={vm.t('system_configs.filters.search_placeholder')}
     className={cn('w-full border-[var(--border-soft)] bg-[var(--bg-surface-hover)]')}
    />
   </div>

   <div className={cn('custom-scrollbar max-h-[70vh] space-y-6 overflow-y-auto px-2 pb-2')}>
    {vm.hasLoadError ? (
     <div className={cn('space-y-4 rounded-2xl border border-red-400/40 bg-red-500/10 px-4 py-5')}>
      <div className={cn('flex items-start gap-3')}>
       <AlertTriangle className={cn('mt-0.5 h-5 w-5 text-red-300')} />
       <p className={cn('text-sm font-semibold text-red-100')}>{vm.loadError}</p>
      </div>
      <Button variant="secondary" onClick={() => {
       void vm.retryLoadConfigs();
      }} className={cn('w-full')}>
       <span className={cn('inline-flex items-center gap-2')}>
        <RefreshCcw className={cn('h-4 w-4')} />
        {vm.t('system_configs.states.retry')}
       </span>
      </Button>
     </div>
    ) : (
     <>
      {vm.groupedItems.map((group) => (
       <div key={group.groupName} className={cn('space-y-3')}>
        <h3 className={cn('sticky top-0 z-10 py-1.5 text-[11px] font-bold uppercase tracking-widest tn-surface tn-text-secondary backdrop-blur-md')}>
         {group.groupName}
        </h3>
        <div className={cn('space-y-2')}>
         {group.items.map((item) => {
          const isSelected = vm.selectedKey === item.key;
          return (
           <button
            key={item.key}
            type="button"
            onClick={() => vm.selectConfig(item)}
            className={cn(
             'w-full rounded-2xl border px-4 py-3 text-left transition-all duration-300',
             isSelected
              ? 'tn-bg-accent-soft tn-border-accent tn-shadow-accent-10'
              : 'tn-surface border-transparent hover:tn-bg-panel-soft hover:tn-border-soft',
            )}
           >
            <div className={cn('flex items-start justify-between gap-2')}>
             <p className={cn('break-all text-sm font-semibold', isSelected ? 'tn-text-accent' : 'tn-text-primary')}>
              {item.key}
             </p>
             <Badge
              variant={item.valueKind.toLowerCase() === 'json' ? 'purple' : 'success'}
              size="sm"
              className={cn('text-[9px] font-bold uppercase')}
             >
              {item.valueKind}
             </Badge>
            </div>
            <p className={cn('mt-2 line-clamp-2 text-xs', isSelected ? 'text-[var(--text-accent-secondary)]' : 'tn-text-tertiary')}>
             {item.description || vm.t('system_configs.meta.no_description')}
            </p>
           </button>
          );
         })}
        </div>
       </div>
      ))}

      {!vm.loading && vm.filteredItems.length === 0 ? (
       <p className={cn('rounded-2xl border border-dashed px-4 py-6 text-center text-sm tn-border-soft tn-text-tertiary')}>
        {vm.t('system_configs.states.empty')}
       </p>
      ) : null}
     </>
    )}
   </div>
  </GlassCard>
 );
}
