import LoadingSpinner from '@/shared/components/ui/LoadingSpinner';

export default function AdminRouteLoading() {
 return (
  <div className="px-4">
   <LoadingSpinner fullPage message="Loading" />
  </div>
 );
}
