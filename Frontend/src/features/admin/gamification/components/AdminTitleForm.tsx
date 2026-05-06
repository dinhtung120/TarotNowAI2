'use client';

import { useEffect } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import type { AdminTitleDefinition } from '@/features/admin/gamification/types/adminGamification.types';
import {
 adminTitleFormSchema,
 toTitleDefinition,
 toTitleFormValues,
 type AdminTitleFormValues,
} from '@/features/admin/gamification/schemas/adminGamificationFormSchema';
import { Button, Input } from '@/shared/ui';
import Modal from '@/shared/ui/Modal';
import { cn } from '@/lib/utils';

interface AdminTitleFormProps {
 initialValue?: AdminTitleDefinition | null;
 open: boolean;
 onClose: () => void;
 onSubmit: (value: AdminTitleDefinition) => Promise<void>;
 submitting: boolean;
}

export function AdminTitleForm({ initialValue, open, onClose, onSubmit, submitting }: AdminTitleFormProps) {
 const { handleSubmit, register, reset, formState: { errors } } = useForm<AdminTitleFormValues>({
  resolver: zodResolver(adminTitleFormSchema),
  defaultValues: toTitleFormValues(initialValue),
 });

 useEffect(() => {
  reset(toTitleFormValues(initialValue));
 }, [initialValue, reset]);

 return (
  <Modal isOpen={open} onClose={onClose} title={initialValue ? 'Edit title' : 'Create title'} size="lg">
   <form className={cn('space-y-4')} onSubmit={handleSubmit(async (values) => onSubmit(toTitleDefinition(values)))}>
    <div className={cn('grid gap-4 md:grid-cols-2')}>
     <Input label="Code" error={errors.code?.message} {...register('code')} />
     <Input label="Rarity" error={errors.rarity?.message} {...register('rarity')} />
     <Input label="Name VI" error={errors.nameVi?.message} {...register('nameVi')} />
     <Input label="Name EN" error={errors.nameEn?.message} {...register('nameEn')} />
     <Input label="Name ZH" error={errors.nameZh?.message} {...register('nameZh')} />
     <label className={cn('flex items-center gap-2 text-sm text-slate-200')}><input type="checkbox" {...register('isActive')} /> Active</label>
    </div>
    <Input label="Description VI" isTextarea error={errors.descriptionVi?.message} {...register('descriptionVi')} />
    <Input label="Description EN" isTextarea error={errors.descriptionEn?.message} {...register('descriptionEn')} />
    <Input label="Description ZH" isTextarea error={errors.descriptionZh?.message} {...register('descriptionZh')} />
    <div className={cn('flex justify-end gap-3')}>
     <Button type="button" variant="secondary" onClick={onClose}>Cancel</Button>
     <Button type="submit" isLoading={submitting}>Save</Button>
    </div>
   </form>
  </Modal>
 );
}
