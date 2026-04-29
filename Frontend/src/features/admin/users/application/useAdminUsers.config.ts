'use client';

import type { Dispatch, SetStateAction } from 'react';
import type { FieldValues, Path, UseFormReturn } from 'react-hook-form';
import { z } from 'zod';
import type { CreateUserParams, UpdateUserParams } from '@/features/admin/application/actions';

export const EMPTY_ADD_FORM: CreateUserParams = {
 email: '',
 username: '',
 displayName: '',
 password: '',
 role: 'user',
};

export const EMPTY_EDIT_FORM: UpdateUserParams = {
 role: 'user',
 status: 'active',
 diamondBalance: 0,
 goldBalance: 0,
};

export const createUserSchema = z.object({
 email: z.string().trim().email(),
 username: z.string().trim().min(3).max(32).regex(/^[a-zA-Z0-9_]+$/),
 displayName: z.string().trim().min(2).max(50),
 password: z.string().min(8).max(100),
 role: z.enum(['user', 'tarot_reader', 'admin']),
});

export const updateUserSchema = z.object({
 role: z.enum(['user', 'tarot_reader', 'admin']),
 status: z.enum(['active', 'locked']),
 diamondBalance: z.number().int().min(0),
 goldBalance: z.number().int().min(0),
});

export type SetStateForm<T> = Dispatch<SetStateAction<T>>;

export function createSetStateForm<T extends FieldValues>(
 form: UseFormReturn<T>,
): SetStateForm<T> {
 return (nextValueOrUpdater) => {
  const current = form.getValues();
  const next = typeof nextValueOrUpdater === 'function'
   ? nextValueOrUpdater(current)
   : nextValueOrUpdater;

  for (const [key, value] of Object.entries(next) as Array<[Path<T>, unknown]>) {
   form.setValue(key, value as never, {
    shouldDirty: true,
    shouldValidate: true,
   });
  }
 };
}
