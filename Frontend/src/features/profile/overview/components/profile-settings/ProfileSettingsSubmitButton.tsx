import { Loader2, Save } from "lucide-react";
import { cn } from "@/lib/utils";
import { Button } from "@/shared/ui";

interface ProfileSettingsSubmitButtonProps {
  isSubmitting: boolean;
  saveLabel: string;
  savingLabel: string;
}

export default function ProfileSettingsSubmitButton({
  isSubmitting,
  saveLabel,
  savingLabel,
}: ProfileSettingsSubmitButtonProps) {
  return (
    <Button
      variant="primary"
      type="submit"
      disabled={isSubmitting}
      className={cn("h-12 w-full")}
    >
      {isSubmitting ? (
        <>
          <Loader2 className={cn("mr-2 h-4 w-4 animate-spin")} />
          {savingLabel}
        </>
      ) : (
        <>
          <Save className={cn("mr-2 h-4 w-4")} />
          {saveLabel}
        </>
      )}
    </Button>
  );
}
