'use client';

import { useEffect } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';
import { useFieldArray, useForm } from 'react-hook-form';
import type { AdminQuestDefinition } from '@/features/gamification/admin/application/adminGamification.types';
import {
 adminQuestFormSchema,
 toQuestDefinition,
 toQuestFormValues,
 type AdminQuestFormValues,
} from '@/features/gamification/admin/application/adminGamificationFormSchema';
import { Button, Input } from '@/shared/components/ui';
import Modal from '@/shared/components/ui/Modal';
import { cn } from '@/lib/utils';

interface AdminQuestFormProps {
 initialValue?: AdminQuestDefinition | null;
 open: boolean;
 onClose: () => void;
 onSubmit: (value: AdminQuestDefinition) => Promise<void>;
 submitting: boolean;
}

export function AdminQuestForm({ initialValue, open, onClose, onSubmit, submitting }: AdminQuestFormProps) {
 const { control, handleSubmit, register, reset, formState: { errors } } = useForm<AdminQuestFormValues>({
  resolver: zodResolver(adminQuestFormSchema),
  defaultValues: toQuestFormValues(initialValue),
 });
 const rewards = useFieldArray({ control, name: 'rewards' });

 useEffect(() => {
  reset(toQuestFormValues(initialValue));
 }, [initialValue, reset]);

 return (
  <Modal isOpen={open} onClose={onClose} title={initialValue ? 'Edit quest' : 'Create quest'} size="lg">
   <form className={cn('space-y-4')} onSubmit={handleSubmit(async (values) => onSubmit(toQuestDefinition(values)))}>
    <div className={cn('grid gap-4 md:grid-cols-2')}>
     <Input label="Code" error={errors.code?.message} {...register('code')} />
     <Input label="Quest type" error={errors.questType?.message} {...register('questType')} />
     <Input label="Title VI" error={errors.titleVi?.message} {...register('titleVi')} />
     <Input label="Title EN" error={errors.titleEn?.message} {...register('titleEn')} />
     <Input label="Title ZH" error={errors.titleZh?.message} {...register('titleZh')} />
     <Input label="Trigger event" error={errors.triggerEvent?.message} {...register('triggerEvent')} />
     <Input label="Target" type="number" error={errors.target?.message} {...register('target', { valueAsNumber: true })} />
     <label className={cn('flex items-center gap-2 text-sm text-slate-200')}><input type="checkbox" {...register('isActive')} /> Active</label>
    </div>
    <Input label="Description VI" isTextarea error={errors.descriptionVi?.message} {...register('descriptionVi')} />
    <Input label="Description EN" isTextarea error={errors.descriptionEn?.message} {...register('descriptionEn')} />
    <Input label="Description ZH" isTextarea error={errors.descriptionZh?.message} {...register('descriptionZh')} />
    <div className={cn('space-y-3')}>
     <div className={cn('flex items-center justify-between')}>
      <h3 className={cn('text-sm font-semibold text-slate-200')}>Rewards</h3>
      <Button type="button" size="sm" onClick={() => rewards.append({ type: '', amount: 1, titleCode: '' })}>Add reward</Button>
     </div>
     {rewards.fields.map((field, index) => (
      <div key={field.id} className={cn('grid gap-3 rounded-lg border border-slate-800 p-3 md:grid-cols-[1fr_120px_1fr_auto]')}>
       <Input label="Type" error={errors.rewards?.[index]?.type?.message} {...register(`rewards.${index}.type`)} />
       <Input label="Amount" type="number" error={errors.rewards?.[index]?.amount?.message} {...register(`rewards.${index}.amount`, { valueAsNumber: true })} />
       <Input label="Title code" error={errors.rewards?.[index]?.titleCode?.message} {...register(`rewards.${index}.titleCode`)} />
       <Button type="button" variant="secondary" className={cn('self-end')} onClick={() => rewards.remove(index)}>Remove</Button>
      </div>
     ))}
    </div>
    <div className={cn('flex justify-end gap-3')}>
     <Button type="button" variant="secondary" onClick={onClose}>Cancel</Button>
     <Button type="submit" isLoading={submitting}>Save</Button>
    </div>
   </form>
  </Modal>
 );
}
