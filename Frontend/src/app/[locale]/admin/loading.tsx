import LoadingSpinner from '@/shared/components/ui/LoadingSpinner';
import { cn } from '@/lib/utils';

export default function AdminRouteLoading() {
 return (
  <div className={cn("px-4")}>
   <LoadingSpinner fullPage message="Loading" />
  </div>
 );
}
