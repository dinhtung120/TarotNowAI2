'use client';

import { act } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { useProfilePage } from '@/features/profile/application/useProfilePage';
import { useHydrateFormOnce } from '@/shared/application/hooks/useHydrateFormOnce';
import { useAuthStore } from '@/store/authStore';
import { useForm } from 'react-hook-form';
import { useQuery, useQueryClient } from '@tanstack/react-query';
import { toast } from 'react-hot-toast';
import { updateProfileAction } from '@/features/profile/application/actions';
import { uploadProfileAvatar } from '@/features/profile/application/uploadProfileAvatar';

vi.mock('@tanstack/react-query', () => ({
 useQuery: vi.fn(),
 useQueryClient: vi.fn(),
}));

vi.mock('next-intl', () => ({
 useTranslations: vi.fn(() => (key: string) => key),
}));

vi.mock('react-hook-form', () => ({
 useForm: vi.fn(),
}));

vi.mock('@/shared/application/hooks/useHydrateFormOnce', () => ({
 useHydrateFormOnce: vi.fn(),
}));

vi.mock('@/store/authStore', () => ({
 useAuthStore: vi.fn(),
}));

vi.mock('@/i18n/routing', () => ({
 useRouter: vi.fn(() => ({ push: vi.fn() })),
}));

vi.mock('@/features/profile/application/actions', () => ({
 getPayoutBanksAction: vi.fn(),
 updateProfileAction: vi.fn(),
}));

vi.mock('@/features/profile/application/uploadProfileAvatar', () => ({
 uploadProfileAvatar: vi.fn(),
}));

vi.mock('@/features/profile/application/profileDetailQuery', () => ({
 fetchProfileDetail: vi.fn(),
 profileDetailQueryKey: ['profile', 'detail'],
}));

vi.mock('@/features/reader/public', () => ({
 getMyReaderRequest: vi.fn(),
}));

vi.mock('react-hot-toast', () => ({
 toast: {
  success: vi.fn(),
  error: vi.fn(),
 },
}));

const mockedUseQuery = vi.mocked(useQuery);
const mockedUseQueryClient = vi.mocked(useQueryClient);
const mockedUseHydrateFormOnce = vi.mocked(useHydrateFormOnce);
const mockedUseAuthStore = vi.mocked(useAuthStore);
const mockedUseForm = vi.mocked(useForm);
const mockedUpdateProfileAction = vi.mocked(updateProfileAction);
const mockedUploadProfileAvatar = vi.mocked(uploadProfileAvatar);
const mockedToast = vi.mocked(toast);

function Harness({ onChange }: { onChange: (value: ReturnType<typeof useProfilePage>) => void }) {
 const value = useProfilePage();
 onChange(value);
 return null;
}

describe('useProfilePage', () => {
 let container: HTMLDivElement;
 let root: Root;

 beforeEach(() => {
  container = document.createElement('div');
  document.body.appendChild(container);
  root = createRoot(container);

  mockedUseQueryClient.mockReturnValue({ invalidateQueries: vi.fn() } as never);
 mockedUseAuthStore.mockImplementation((selector) => selector({
   isAuthenticated: true,
   user: { role: 'user' },
  } as never));
  mockedUseAuthStore.getState = vi.fn(() => ({
   updateUser: vi.fn(),
  })) as never;
  mockedUseForm.mockReturnValue({
   register: vi.fn(),
   handleSubmit: vi.fn(),
   reset: vi.fn(),
   formState: {
    errors: {},
    isSubmitting: false,
   },
  } as never);

  mockedUseQuery.mockImplementation(({ queryKey }) => {
   const normalizedKey = JSON.stringify(queryKey);
   if (normalizedKey === JSON.stringify(['profile', 'detail'])) {
    return {
     data: {
      profile: {
       id: 'user-1',
       displayName: 'User One',
       payoutBankBin: null,
       payoutBankAccountNumber: null,
       payoutBankAccountHolder: null,
       dateOfBirth: '2000-01-02T00:00:00Z',
       avatarUrl: 'https://cdn.test/avatar.webp',
      },
      error: '',
     },
     isLoading: false,
     isFetching: false,
    } as never;
   }

   if (normalizedKey === JSON.stringify(['profile', 'payout-banks'])) {
    return {
     data: { options: [], error: '' },
     isLoading: false,
     isFetching: false,
    } as never;
   }

   return {
    data: null,
    isLoading: false,
    isFetching: false,
   } as never;
  });
 });

 afterEach(() => {
  act(() => root.unmount());
  container.remove();
  vi.clearAllMocks();
 });

 it('hydrates profile form through the dirty-safe helper with stable identity and values', () => {
  let latest: ReturnType<typeof useProfilePage> | null = null;

  act(() => {
   root.render(<Harness onChange={(value) => {
    latest = value;
   }} />);
  });

  expect(mockedUseHydrateFormOnce).toHaveBeenCalledWith(expect.objectContaining({
   identity: 'user-1',
   values: {
    displayName: 'User One',
    payoutBankBin: '',
    payoutBankAccountNumber: '',
    payoutBankAccountHolder: '',
    dateOfBirth: '2000-01-02',
   },
  }));
  expect(latest?.profileData?.displayName).toBe('User One');
  expect(latest?.avatarPreview).toBe('https://cdn.test/avatar.webp');
 });

 it('updates profile state on successful submit and handles avatar uploads', async () => {
  const invalidateQueries = vi.fn().mockResolvedValue(undefined);
  const updateUser = vi.fn();
  mockedUseQueryClient.mockReturnValue({ invalidateQueries } as never);
  mockedUseAuthStore.getState = vi.fn(() => ({ updateUser })) as never;
  mockedUpdateProfileAction.mockResolvedValue({ success: true } as never);
  mockedUploadProfileAvatar.mockResolvedValue({
   success: true,
   avatarUrl: 'https://cdn.test/new-avatar.webp',
   message: 'Avatar updated.',
  } as never);
  vi.spyOn(URL, 'createObjectURL').mockReturnValue('blob:test');
  vi.spyOn(URL, 'revokeObjectURL').mockImplementation(() => undefined);

  let latest: ReturnType<typeof useProfilePage> | null = null;
  act(() => {
   root.render(<Harness onChange={(value) => {
    latest = value;
   }} />);
  });

  await act(async () => {
   await latest?.onSubmit({
    displayName: 'Updated User',
    dateOfBirth: '2000-01-02',
    payoutBankBin: '',
    payoutBankAccountNumber: '',
    payoutBankAccountHolder: '',
   });
  });

  expect(updateUser).toHaveBeenCalledWith({ displayName: 'Updated User', avatarUrl: 'https://cdn.test/avatar.webp' });
  expect(invalidateQueries).toHaveBeenCalledTimes(2);

  const file = new File(['avatar'], 'avatar.png', { type: 'image/png' });
  await act(async () => {
   await latest?.handleAvatarSelect({
    target: {
     files: [file],
     value: 'avatar.png',
    },
   } as React.ChangeEvent<HTMLInputElement>);
  });

  expect(mockedToast.success).toHaveBeenCalledWith('Avatar updated.');
  expect(updateUser).toHaveBeenCalledWith({ avatarUrl: 'https://cdn.test/new-avatar.webp' });
 });
});
