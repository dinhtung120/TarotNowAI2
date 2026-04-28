'use client';

import { useEffect } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import type { AdminAchievementDefinition } from '@/features/gamification/admin/adminGamification.types';
import {
 adminAchievementFormSchema,
 toAchievementDefinition,
 toAchievementFormValues,
 type AdminAchievementFormValues,
} from '@/features/gamification/adminGamificationFormSchema';
import { Button, Input } from '@/shared/components/ui';
import Modal from '@/shared/components/ui/Modal';
import { cn } from '@/lib/utils';

interface AdminAchievementFormProps {
 initialValue?: AdminAchievementDefinition | null;
 open: boolean;
 onClose: () => void;
 onSubmit: (value: AdminAchievementDefinition) => Promise<void>;
 submitting: boolean;
}

export function AdminAchievementForm({
 initialValue,
 open,
 onClose,
 onSubmit,
 submitting,
}: AdminAchievementFormProps) {
 const { handleSubmit, register, reset, formState: { errors } } = useForm<AdminAchievementFormValues>({
  resolver: zodResolver(adminAchievementFormSchema),
  defaultValues: toAchievementFormValues(initialValue),
 });

 useEffect(() => {
  reset(toAchievementFormValues(initialValue));
 }, [initialValue, reset]);

 return (
  <Modal isOpen={open} onClose={onClose} title={initialValue ? 'Edit achievement' : 'Create achievement'} size="lg">
   <form className={cn('space-y-4')} onSubmit={handleSubmit(async (values) => onSubmit(toAchievementDefinition(values)))}>
    <div className={cn('grid gap-4 md:grid-cols-2')}>
     <Input label="Code" error={errors.code?.message} {...register('code')} />
     <Input label="Icon" error={errors.icon?.message} {...register('icon')} />
     <Input label="Title VI" error={errors.titleVi?.message} {...register('titleVi')} />
     <Input label="Title EN" error={errors.titleEn?.message} {...register('titleEn')} />
     <Input label="Title ZH" error={errors.titleZh?.message} {...register('titleZh')} />
     <Input label="Granted title code" error={errors.grantsTitleCode?.message} {...register('grantsTitleCode')} />
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
