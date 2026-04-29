import { Save, Settings } from 'lucide-react';
import { cn } from '@/lib/utils';
import { Button, GlassCard } from '@/shared/components/ui';
import type { AdminSystemConfigsViewModel } from '@/features/admin/system-configs/application/useAdminSystemConfigs';
import SystemConfigsLoadErrorState from '@/features/admin/system-configs/presentation/SystemConfigsLoadErrorState';

interface SystemConfigsEditorPanelProps {
 vm: AdminSystemConfigsViewModel;
 selectedUpdatedAt: string;
}

export default function SystemConfigsEditorPanel({ vm, selectedUpdatedAt }: SystemConfigsEditorPanelProps) {
 return (
  <GlassCard padding="lg" className={cn('self-start tn-panel shadow-[var(--shadow-card)]')}>
   {vm.hasLoadError ? (
    <SystemConfigsLoadErrorState vm={vm} />
   ) : vm.selectedItem ? (
    <div className={cn('space-y-6')}>
     <div className={cn('grid gap-6 md:grid-cols-2')}>
      <div>
       <p className={cn('text-xs font-bold uppercase tracking-wider tn-text-tertiary')}>{vm.t('system_configs.form.key')}</p>
       <p className={cn('mt-2 break-all rounded-xl border px-4 py-3 text-sm font-medium tn-border-soft tn-surface tn-text-primary')}>
        {vm.selectedItem.key}
       </p>
      </div>
      <label>
       <span className={cn('text-xs font-bold uppercase tracking-wider tn-text-tertiary')}>
        {vm.t('system_configs.form.value_kind')}
       </span>
       <select
        value={vm.draftValueKind}
        onChange={(event) => vm.setDraftValueKind(event.target.value === 'json' ? 'json' : 'scalar')}
        className={cn('mt-2 w-full rounded-xl border px-4 py-3 text-sm outline-none transition-colors focus:tn-border-accent tn-border-soft tn-panel-soft tn-text-primary')}
       >
        <option value="scalar" className={cn('bg-[var(--bg-panel)]')}>
         scalar
        </option>
        <option value="json" className={cn('bg-[var(--bg-panel)]')}>
         json
        </option>
       </select>
     </label>
     </div>
     <label className={cn('block')}>
      <span className={cn('text-xs font-bold uppercase tracking-wider tn-text-tertiary')}>
       {vm.t('system_configs.form.description')}
      </span>
      <textarea
       value={vm.draftDescription}
       onChange={(event) => vm.setDraftDescription(event.target.value)}
       rows={3}
       className={cn('mt-2 w-full resize-y rounded-xl border px-4 py-3 text-sm outline-none transition-colors placeholder:text-[var(--text-tertiary)] focus:tn-border-accent tn-border-soft tn-panel-soft tn-text-primary')}
      placeholder={vm.t('system_configs.form.description_placeholder')}
      />
     </label>
     <label className={cn('block')}>
      <span className={cn('text-xs font-bold uppercase tracking-wider tn-text-tertiary')}>
       {vm.t('system_configs.form.value')}
      </span>
      <textarea
       value={vm.draftValue}
       onChange={(event) => vm.setDraftValue(event.target.value)}
       rows={16}
       className={cn('custom-scrollbar mt-2 w-full resize-y rounded-xl border bg-[#0f111a]/80 px-4 py-4 font-mono text-[13px] leading-relaxed outline-none transition-colors placeholder:text-[var(--text-tertiary)] focus:tn-border-accent tn-border-soft tn-text-primary')}
       placeholder={vm.t('system_configs.form.value_placeholder')}
      spellCheck={false}
     />
     </label>
     <div className={cn('flex flex-wrap items-center justify-between gap-4 border-t pt-4 tn-border-soft')}>
      <div className={cn('text-xs tn-text-tertiary')}>
       {selectedUpdatedAt
        ? vm.t('system_configs.meta.updated_at', { value: selectedUpdatedAt })
        : vm.t('system_configs.meta.never_updated')}
      </div>
      <Button
       variant="primary"
       disabled={vm.saving}
       onClick={vm.saveSelectedConfig}
       className={cn('px-6 py-2.5 tn-shadow-accent-20')}
      >
       <span className={cn('inline-flex items-center gap-2')}>
        <Save className={cn('h-4 w-4')} />
        {vm.saving ? vm.t('system_configs.actions.saving') : vm.t('system_configs.actions.save')}
       </span>
      </Button>
     </div>
    </div>
   ) : (
    <div className={cn('flex flex-col items-center justify-center py-20 text-center')}>
     <div className={cn('mb-4 flex h-16 w-16 items-center justify-center rounded-full tn-panel-soft')}>
      <Settings className={cn('h-8 w-8 opacity-50 tn-text-tertiary')} />
     </div>
     <p className={cn('text-sm tn-text-tertiary')}>{vm.t('system_configs.states.select_prompt')}</p>
    </div>
   )}
  </GlassCard>
 );
}
