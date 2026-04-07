import { Link } from "@/i18n/routing";
import { Compass, Users } from "lucide-react";
import { cn } from "@/lib/utils";
import { Button } from "@/shared/components/ui";

interface HeroActionsProps {
  ctaDraw: string;
  ctaMeetReaders: string;
}

export default function HeroActions({
  ctaDraw,
  ctaMeetReaders,
}: HeroActionsProps) {
  return (
    <div
      className={cn(
        "animate-in fade-in slide-in-from-bottom-12 relative z-10 flex w-full flex-col items-center gap-6 delay-700 duration-1000 sm:w-auto sm:flex-row",
      )}
    >
      <Link href="/reading" tabIndex={-1}>
        <Button
          size="lg"
          className={cn("group shadow-[var(--glow-white)]")}
          rightIcon={
            <Compass
              className={cn(
                "ml-2 h-5 w-5 transition-transform duration-700 group-hover:rotate-180",
              )}
            />
          }
        >
          {ctaDraw}
        </Button>
      </Link>
      <Link href="/readers" tabIndex={-1}>
        <Button
          variant="secondary"
          size="lg"
          className={cn("group")}
          rightIcon={
            <Users
              className={cn(
                "ml-2 h-5 w-5 transition-transform duration-300 group-hover:translate-x-1",
              )}
            />
          }
        >
          {ctaMeetReaders}
        </Button>
      </Link>
    </div>
  );
}
