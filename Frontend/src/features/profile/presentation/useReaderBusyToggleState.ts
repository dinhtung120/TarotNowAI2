import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import toast from "react-hot-toast";
import { updateReaderStatus, getReaderProfile } from "@/features/reader/public";
import {
  normalizeReaderStatus,
  type ReaderStatus,
} from "@/features/reader/domain/readerStatus";
import { useAuthStore } from "@/store/authStore";

export function useReaderBusyToggleState() {
  const user = useAuthStore((state) => state.user);
  const queryClient = useQueryClient();

  const profileQuery = useQuery({
    queryKey: ["reader-profile-settings", user?.id],
    enabled: Boolean(user),
    queryFn: async () => {
      if (!user) return null;
      const result = await getReaderProfile(user.id);
      return result.success ? (result.data ?? null) : null;
    },
  });

  const statusMutation = useMutation({
    mutationFn: updateReaderStatus,
  });

  const currentStatus = normalizeReaderStatus(profileQuery.data?.status);
  const isBusy = currentStatus === "busy";
  const isLoading = profileQuery.isLoading || statusMutation.isPending;

  const handleToggle = async () => {
    const nextStatus: ReaderStatus = isBusy ? "offline" : "busy";
    const result = await statusMutation.mutateAsync(nextStatus);
    if (result.success) {
      await queryClient.invalidateQueries({
        queryKey: ["reader-profile-settings", user?.id],
      });
      return;
    }

    toast.error('Cập nhật trạng thái thất bại', {
      className: 'tn-toast tn-toast-error tn-toast-reader-status',
    });
  };

  return {
    handleToggle,
    isBusy,
    isLoading,
    statusText: isBusy ? "Bận" : "Sẵn sàng",
    title: isBusy
      ? "Chuyển sang trạng thái Online (Sẵn sàng)"
      : "Chuyển sang trạng thái Bận",
  };
}
