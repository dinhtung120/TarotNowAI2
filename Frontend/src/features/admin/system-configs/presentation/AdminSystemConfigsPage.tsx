'use client';

import { Save, Search, Power } from 'lucide-react';
import { cn } from '@/lib/utils';
import { useAdminSystemConfigs } from '@/features/admin/system-configs/application/useAdminSystemConfigs';
import type { AdminSystemConfigItem } from '@/features/admin/system-configs/system-config.types';

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
  <div className={cn('space-y-6', 'pb-20', 'animate-in', 'fade-in', 'duration-500')}>
   <section className={cn('flex', 'flex-wrap', 'items-center', 'justify-between', 'gap-4', 'rounded-3xl', 'border', 'border-slate-200', 'bg-white/90', 'p-6', 'shadow-sm')}>
    <div>
     <p className={cn('text-xs', 'font-semibold', 'uppercase', 'tracking-[0.18em]', 'text-cyan-700')}>
      {vm.t('system_configs.header.tag')}
     </p>
     <h1 className={cn('mt-2', 'text-2xl', 'font-semibold', 'text-slate-900')}>
      {vm.t('system_configs.header.title')}
     </h1>
     <p className={cn('mt-2', 'text-sm', 'text-slate-600')}>
      {vm.t('system_configs.header.subtitle', { count: vm.items.length })}
     </p>
    </div>
    <button
     type="button"
     disabled={vm.restarting}
     onClick={() => {
      // Vui lòng thêm các key vào file lang en.json/vi.json của bạn: system_configs.actions.confirm_restart, system_configs.actions.restarting, system_configs.actions.restart
      if (window.confirm(vm.t('system_configs.actions.confirm_restart') || 'Are you sure you want to restart the server?')) {
       vm.handleRestartServer();
      }
     }}
     className={cn(
      'inline-flex',
      'items-center',
      'gap-2',
      'rounded-xl',
      'bg-red-500',
      'px-4',
      'py-2.5',
      'text-sm',
      'font-semibold',
      'text-white',
      'transition',
      'hover:bg-red-600',
      'disabled:cursor-not-allowed',
      'disabled:opacity-50'
     )}
    >
     <Power className="h-4 w-4" />
     {vm.restarting ? (vm.t('system_configs.actions.restarting') || 'Restarting...') : (vm.t('system_configs.actions.restart') || 'Restart Server')}
    </button>
   </section>

   <section className={cn('grid', 'gap-4', 'xl:grid-cols-[380px_minmax(0,1fr)]')}>
    <aside className={cn('rounded-3xl', 'border', 'border-slate-200', 'bg-white', 'p-4', 'shadow-sm')}>
     <label className={cn('mb-3', 'flex', 'items-center', 'gap-2', 'rounded-2xl', 'border', 'border-slate-200', 'bg-slate-50', 'px-3', 'py-2')}>
      <Search className={cn('h-4', 'w-4', 'text-slate-500')} />
      <input
       value={vm.searchText}
       onChange={(event) => vm.setSearchText(event.target.value)}
       placeholder={vm.t('system_configs.filters.search_placeholder')}
       className={cn('w-full', 'border-none', 'bg-transparent', 'text-sm', 'outline-none')}
      />
     </label>
     <div className={cn('max-h-[70vh]', 'space-y-6', 'overflow-y-auto', 'pr-2')}>
      {vm.groupedItems.map((group) => (
       <div key={group.groupName} className="space-y-3">
        <h3 className={cn('sticky', 'top-0', 'z-10', 'bg-white/95', 'py-1.5', 'backdrop-blur', 'text-[11px]', 'font-bold', 'uppercase', 'tracking-widest', 'text-slate-400')}>
         {group.groupName}
        </h3>
        <div className="space-y-2">
         {group.items.map((item) => (
          <button
           key={item.key}
           type="button"
           onClick={() => vm.selectConfig(item)}
           className={cn(
            'w-full',
            'rounded-2xl',
            'border',
            'px-3',
            'py-3',
            'text-left',
            'transition',
            vm.selectedKey === item.key
             ? 'border-cyan-500 bg-cyan-50 shadow-sm'
             : 'border-slate-200 bg-white hover:border-cyan-300 hover:bg-cyan-50/40',
           )}
          >
           <div className={cn('flex', 'items-start', 'justify-between', 'gap-2')}>
            <p className={cn('text-sm', 'font-semibold', 'text-slate-900', 'break-all')}>{item.key}</p>
            <span
             className={cn(
              'rounded-full',
              'px-2',
              'py-1',
              'text-[10px]',
              'font-semibold',
              'uppercase',
              item.valueKind.toLowerCase() === 'json'
               ? 'bg-indigo-100 text-indigo-700'
               : 'bg-emerald-100 text-emerald-700',
             )}
            >
             {item.valueKind}
            </span>
           </div>
           <p className={cn('mt-2', 'line-clamp-2', 'text-xs', 'text-slate-600')}>
            {item.description || vm.t('system_configs.meta.no_description')}
           </p>
          </button>
         ))}
        </div>
       </div>
      ))}
      {!vm.loading && vm.filteredItems.length === 0 ? (
       <p className={cn('rounded-2xl', 'border', 'border-dashed', 'border-slate-300', 'px-3', 'py-4', 'text-center', 'text-sm', 'text-slate-500')}>
        {vm.t('system_configs.states.empty')}
       </p>
      ) : null}
     </div>
    </aside>

    <div className={cn('rounded-3xl', 'border', 'border-slate-200', 'bg-white', 'p-6', 'shadow-sm')}>
     {vm.selectedItem ? (
      <div className={cn('space-y-4')}>
       <div className={cn('grid', 'gap-4', 'md:grid-cols-2')}>
        <div>
         <p className={cn('text-xs', 'font-semibold', 'uppercase', 'tracking-wide', 'text-slate-500')}>
          {vm.t('system_configs.form.key')}
         </p>
         <p className={cn('mt-2', 'break-all', 'rounded-xl', 'bg-slate-100', 'px-3', 'py-2', 'text-sm', 'font-medium', 'text-slate-900')}>
          {vm.selectedItem.key}
         </p>
        </div>
        <label>
         <span className={cn('text-xs', 'font-semibold', 'uppercase', 'tracking-wide', 'text-slate-500')}>
          {vm.t('system_configs.form.value_kind')}
         </span>
         <select
          value={vm.draftValueKind}
          onChange={(event) => vm.setDraftValueKind(event.target.value === 'json' ? 'json' : 'scalar')}
          className={cn('mt-2', 'w-full', 'rounded-xl', 'border', 'border-slate-300', 'bg-white', 'px-3', 'py-2', 'text-sm', 'outline-none focus:border-cyan-500')}
         >
          <option value="scalar">scalar</option>
          <option value="json">json</option>
         </select>
        </label>
       </div>

       <label className={cn('block')}>
        <span className={cn('text-xs', 'font-semibold', 'uppercase', 'tracking-wide', 'text-slate-500')}>
         {vm.t('system_configs.form.description')}
        </span>
        <textarea
         value={vm.draftDescription}
         onChange={(event) => vm.setDraftDescription(event.target.value)}
         rows={3}
         className={cn('mt-2', 'w-full', 'resize-y', 'rounded-xl', 'border', 'border-slate-300', 'px-3', 'py-2', 'text-sm', 'outline-none focus:border-cyan-500')}
         placeholder={vm.t('system_configs.form.description_placeholder')}
        />
       </label>

       <label className={cn('block')}>
        <span className={cn('text-xs', 'font-semibold', 'uppercase', 'tracking-wide', 'text-slate-500')}>
         {vm.t('system_configs.form.value')}
        </span>
        <textarea
         value={vm.draftValue}
         onChange={(event) => vm.setDraftValue(event.target.value)}
         rows={16}
         className={cn('mt-2', 'w-full', 'resize-y', 'rounded-xl', 'border', 'border-slate-300', 'font-mono', 'text-xs', 'leading-6', 'px-3', 'py-2', 'outline-none focus:border-cyan-500')}
         placeholder={vm.t('system_configs.form.value_placeholder')}
        />
       </label>

       <div className={cn('flex', 'flex-wrap', 'items-center', 'justify-between', 'gap-3', 'pt-2')}>
        <div className={cn('text-xs', 'text-slate-600')}>
         {selectedUpdatedAt
          ? vm.t('system_configs.meta.updated_at', { value: selectedUpdatedAt })
          : vm.t('system_configs.meta.never_updated')}
        </div>
        <button
         type="button"
         disabled={vm.saving}
         onClick={vm.saveSelectedConfig}
         className={cn(
          'inline-flex',
          'items-center',
          'gap-2',
          'rounded-xl',
          'bg-cyan-600',
          'px-4',
          'py-2',
          'text-sm',
          'font-semibold',
          'text-white',
          'transition',
          'hover:bg-cyan-700',
          'disabled:cursor-not-allowed',
          'disabled:bg-cyan-300',
         )}
        >
         <Save className={cn('h-4', 'w-4')} />
         {vm.saving ? vm.t('system_configs.actions.saving') : vm.t('system_configs.actions.save')}
        </button>
       </div>
      </div>
     ) : (
      <p className={cn('rounded-2xl', 'border', 'border-dashed', 'border-slate-300', 'px-3', 'py-4', 'text-sm', 'text-slate-500')}>
       {vm.t('system_configs.states.select_prompt')}
      </p>
     )}
    </div>
   </section>
  </div>
 );
}
