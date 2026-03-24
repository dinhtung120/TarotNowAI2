'use client';

import { useEffect, useState, type FormEvent } from 'react';
import { useRouter } from '@/i18n/routing';
import { useAuthStore } from '@/store/authStore';
import {
  getReaderProfile,
  updateReaderProfile,
  updateReaderStatus,
} from '@/features/reader/public';
import toast from 'react-hot-toast';

type TranslateFn = (key: string, values?: Record<string, string | number | Date>) => string;

export function useProfileReaderSettingsPage(t: TranslateFn) {
  const router = useRouter();
  const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
  const user = useAuthStore((state) => state.user);

  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [bioVi, setBioVi] = useState('');
  const [diamondPerQuestion, setDiamondPerQuestion] = useState<number>(100);
  const [specialtiesStr, setSpecialtiesStr] = useState('');
  const [status, setStatus] = useState('offline');

  useEffect(() => {
    if (!isAuthenticated || !user) {
      router.push('/login');
      return;
    }

    const fetchProfile = async () => {
      const result = await getReaderProfile(user.id);
      if (result.success && result.data) {
        setBioVi(result.data.bioVi || '');
        setDiamondPerQuestion(result.data.diamondPerQuestion || 100);
        setSpecialtiesStr(result.data.specialties?.join(', ') || '');
        setStatus(result.data.status || 'offline');
      } else {
        toast.error(t('reader.toast_not_found'), {
          style: {
            background: 'var(--bg-elevated)',
            color: 'var(--text-primary)',
            border: '1px solid var(--border-default)',
          },
        });
        router.push('/profile');
      }
      setLoading(false);
    };

    void fetchProfile();
  }, [isAuthenticated, user, router, t]);

  const handleSave = async (event: FormEvent) => {
    event.preventDefault();
    setSaving(true);

    const specArray = specialtiesStr
      .split(',')
      .map((item) => item.trim())
      .filter((item) => item.length > 0);

    const result = await updateReaderProfile({
      bioVi,
      diamondPerQuestion,
      specialties: specArray,
    });

    if (result.success) {
      toast.success(t('reader.toast_save_success'), {
        style: {
          background: 'var(--success-bg)',
          color: 'var(--success)',
          border: '1px solid var(--success)',
        },
      });
    } else {
      toast.error(t('reader.toast_save_fail'), {
        style: {
          background: 'var(--danger-bg)',
          color: 'var(--danger)',
          border: '1px solid var(--danger)',
        },
      });
    }

    setSaving(false);
  };

  const handleStatusChange = async (newStatus: string) => {
    setStatus(newStatus);
    const result = await updateReaderStatus(newStatus);
    if (result.success) {
      toast.success(t('reader.toast_status_updated'), {
        style: {
          background: 'var(--success-bg)',
          color: 'var(--success)',
          border: '1px solid var(--success)',
        },
      });
    } else {
      toast.error(t('reader.toast_status_update_fail'), {
        style: {
          background: 'var(--danger-bg)',
          color: 'var(--danger)',
          border: '1px solid var(--danger)',
        },
      });
    }
  };

  return {
    loading,
    saving,
    bioVi,
    setBioVi,
    diamondPerQuestion,
    setDiamondPerQuestion,
    specialtiesStr,
    setSpecialtiesStr,
    status,
    handleSave,
    handleStatusChange,
  };
}
