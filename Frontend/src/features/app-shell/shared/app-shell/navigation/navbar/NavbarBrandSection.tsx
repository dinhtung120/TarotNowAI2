import { OptimizedLink as Link } from '@/shared/navigation/useOptimizedLink';
import { NavbarDesktopLinks } from '@/features/app-shell/shared/app-shell/navigation/navbar/NavbarDesktopLinks';
import { NAV_LINKS } from '@/shared/app-shell/navigation/navbar/config';
import { cn } from '@/lib/utils';

interface NavbarBrandSectionProps {
  pathname: string;
  tNav: (key: string) => string;
}

export default function NavbarBrandSection({ pathname, tNav }: NavbarBrandSectionProps) {
  return (
    <div className={cn('flex items-center gap-3')}>
      <Link href="/" className={cn('inline-flex min-h-11 items-center px-1 text-lg font-black italic tracking-tighter lunar-metallic-text')}>
        TarotNow AI
      </Link>
      <NavbarDesktopLinks links={NAV_LINKS} pathname={pathname} tNav={tNav} />
    </div>
  );
}
