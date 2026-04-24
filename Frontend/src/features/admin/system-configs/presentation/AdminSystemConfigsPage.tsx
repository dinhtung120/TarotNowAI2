'use client';

import { Save, Search, Power, Settings } from 'lucide-react';
import { cn } from '@/lib/utils';
import { useAdminSystemConfigs } from '@/features/admin/system-configs/application/useAdminSystemConfigs';
import type { AdminSystemConfigItem } from '@/features/admin/system-configs/system-config.types';
import { Button, Input, GlassCard, SectionHeader, Badge } from '@/shared/components/ui';

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

 return (
  <div className={cn('space-y-8', 'pb-20', 'animate-in', 'fade-in', 'duration-700')}>
   <SectionHeader
    tag={vm.t('system_configs.header.tag')}
    tagIcon={<Settings className={cn('w-3 h-3 tn-text-accent')} />}
    title={vm.t('system_configs.header.title')}
    subtitle={vm.t('system_configs.header.subtitle', { count: vm.items.length })}
    className={cn('text-left items-start')}
    action={
     <Button
      variant="danger"
      disabled={vm.restarting}
      onClick={() => {
       if (window.confirm(vm.t('system_configs.actions.confirm_restart') || 'Are you sure you want to restart the server?')) {
        vm.handleRestartServer();
       }
      }}
      className={cn('px-5 py-3 whitespace-nowrap tn-shadow-accent-20')}
     >
      <span className={cn('inline-flex items-center gap-2')}>
       <Power className={cn('w-4 h-4')} />
       {vm.restarting ? (vm.t('system_configs.actions.restarting') || 'Restarting...') : (vm.t('system_configs.actions.restart') || 'Restart Server')}
      </span>
     </Button>
    }
   />

   <div className={cn('grid', 'gap-6', 'xl:grid-cols-[380px_minmax(0,1fr)]')}>
    <GlassCard padding="sm" className={cn('flex flex-col gap-4 tn-panel shadow-[var(--shadow-card)]')}>
     <div className={cn('px-2 pt-2')}>
      <Input
       leftIcon={<Search className={cn('w-4 h-4')} />}
       value={vm.searchText}
       onChange={(event) => vm.setSearchText(event.target.value)}
       placeholder={vm.t('system_configs.filters.search_placeholder')}
       className={cn('w-full bg-[var(--bg-surface-hover)] border-[var(--border-soft)]')}
      />
     </div>
     
     <div className={cn('max-h-[70vh]', 'space-y-6', 'overflow-y-auto', 'px-2', 'pb-2', 'custom-scrollbar')}>
      {vm.groupedItems.map((group) => (
       <div key={group.groupName} className="space-y-3">
        <h3 className={cn('sticky', 'top-0', 'z-10', 'tn-surface', 'py-1.5', 'backdrop-blur-md', 'text-[11px]', 'font-bold', 'uppercase', 'tracking-widest', 'tn-text-secondary')}>
         {group.groupName}
        </h3>
        <div className="space-y-2">
         {group.items.map((item) => {
          const isSelected = vm.selectedKey === item.key;
          return (
           <button
            key={item.key}
            type="button"
            onClick={() => vm.selectConfig(item)}
            className={cn(
             'w-full',
             'rounded-2xl',
             'border',
             'px-4',
             'py-3',
             'text-left',
             'transition-all',
             'duration-300',
             isSelected
              ? 'tn-border-accent tn-bg-accent-soft tn-shadow-accent-10'
              : 'border-transparent tn-surface hover:tn-border-soft hover:tn-bg-panel-soft',
            )}
           >
            <div className={cn('flex', 'items-start', 'justify-between', 'gap-2')}>
             <p className={cn('text-sm', 'font-semibold', isSelected ? 'tn-text-accent' : 'tn-text-primary', 'break-all')}>
              {item.key}
             </p>
             <Badge variant={item.valueKind.toLowerCase() === 'json' ? 'purple' : 'success'} size="sm" className="uppercase text-[9px] font-bold">
              {item.valueKind}
             </Badge>
            </div>
            <p className={cn('mt-2', 'line-clamp-2', 'text-xs', isSelected ? 'text-[var(--text-accent-secondary)]' : 'tn-text-tertiary')}>
             {item.description || vm.t('system_configs.meta.no_description')}
            </p>
           </button>
          );
         })}
        </div>
       </div>
      ))}
      {!vm.loading && vm.filteredItems.length === 0 ? (
       <p className={cn('rounded-2xl', 'border', 'border-dashed', 'tn-border-soft', 'px-4', 'py-6', 'text-center', 'text-sm', 'tn-text-tertiary')}>
        {vm.t('system_configs.states.empty')}
       </p>
      ) : null}
     </div>
    </GlassCard>

    <GlassCard padding="lg" className={cn('tn-panel shadow-[var(--shadow-card)] self-start')}>
     {vm.selectedItem ? (
      <div className={cn('space-y-6')}>
       <div className={cn('grid', 'gap-6', 'md:grid-cols-2')}>
        <div>
         <p className={cn('text-xs', 'font-bold', 'uppercase', 'tracking-wider', 'tn-text-tertiary')}>
          {vm.t('system_configs.form.key')}
         </p>
         <p className={cn('mt-2', 'break-all', 'rounded-xl', 'tn-surface', 'border', 'tn-border-soft', 'px-4', 'py-3', 'text-sm', 'font-medium', 'tn-text-primary')}>
          {vm.selectedItem.key}
         </p>
        </div>
        <label>
         <span className={cn('text-xs', 'font-bold', 'uppercase', 'tracking-wider', 'tn-text-tertiary')}>
          {vm.t('system_configs.form.value_kind')}
         </span>
         <select
          value={vm.draftValueKind}
          onChange={(event) => vm.setDraftValueKind(event.target.value === 'json' ? 'json' : 'scalar')}
          className={cn('mt-2', 'w-full', 'rounded-xl', 'border', 'tn-border-soft', 'tn-panel-soft', 'px-4', 'py-3', 'text-sm', 'tn-text-primary', 'outline-none', 'focus:tn-border-accent', 'transition-colors')}
         >
          <option value="scalar" className="bg-[var(--bg-panel)]">scalar</option>
          <option value="json" className="bg-[var(--bg-panel)]">json</option>
         </select>
        </label>
       </div>

       <label className={cn('block')}>
        <span className={cn('text-xs', 'font-bold', 'uppercase', 'tracking-wider', 'tn-text-tertiary')}>
         {vm.t('system_configs.form.description')}
        </span>
        <textarea
         value={vm.draftDescription}
         onChange={(event) => vm.setDraftDescription(event.target.value)}
         rows={3}
         className={cn('mt-2', 'w-full', 'resize-y', 'rounded-xl', 'border', 'tn-border-soft', 'tn-panel-soft', 'px-4', 'py-3', 'text-sm', 'tn-text-primary', 'outline-none', 'focus:tn-border-accent', 'transition-colors', 'placeholder:text-[var(--text-tertiary)]')}
         placeholder={vm.t('system_configs.form.description_placeholder')}
        />
       </label>

       <label className={cn('block')}>
        <span className={cn('text-xs', 'font-bold', 'uppercase', 'tracking-wider', 'tn-text-tertiary')}>
         {vm.t('system_configs.form.value')}
        </span>
        <textarea
         value={vm.draftValue}
         onChange={(event) => vm.setDraftValue(event.target.value)}
         rows={16}
         className={cn('mt-2', 'w-full', 'resize-y', 'rounded-xl', 'border', 'tn-border-soft', 'bg-[#0f111a]/80', 'font-mono', 'text-[13px]', 'leading-relaxed', 'px-4', 'py-4', 'tn-text-primary', 'outline-none', 'focus:tn-border-accent', 'transition-colors', 'placeholder:text-[var(--text-tertiary)]', 'custom-scrollbar')}
         placeholder={vm.t('system_configs.form.value_placeholder')}
         spellCheck={false}
        />
       </label>

       <div className={cn('flex', 'flex-wrap', 'items-center', 'justify-between', 'gap-4', 'pt-4', 'border-t', 'tn-border-soft')}>
        <div className={cn('text-xs', 'tn-text-tertiary')}>
         {selectedUpdatedAt
          ? vm.t('system_configs.meta.updated_at', { value: selectedUpdatedAt })
          : vm.t('system_configs.meta.never_updated')}
        </div>
        <Button
         variant="primary"
         disabled={vm.saving}
         onClick={vm.saveSelectedConfig}
         className={cn('px-6', 'py-2.5', 'tn-shadow-accent-20')}
        >
         <span className={cn('inline-flex', 'items-center', 'gap-2')}>
          <Save className={cn('h-4', 'w-4')} />
          {vm.saving ? vm.t('system_configs.actions.saving') : vm.t('system_configs.actions.save')}
         </span>
        </Button>
       </div>
      </div>
     ) : (
      <div className={cn('flex', 'flex-col', 'items-center', 'justify-center', 'py-20', 'text-center')}>
       <div className={cn('w-16', 'h-16', 'rounded-full', 'tn-panel-soft', 'flex', 'items-center', 'justify-center', 'mb-4')}>
        <Settings className={cn('w-8', 'h-8', 'tn-text-tertiary', 'opacity-50')} />
       </div>
       <p className={cn('text-sm', 'tn-text-tertiary')}>
        {vm.t('system_configs.states.select_prompt')}
       </p>
      </div>
     )}
    </GlassCard>
   </div>
  </div>
 );
}
