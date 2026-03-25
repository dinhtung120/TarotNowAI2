import LoadingSpinner from '@/shared/components/ui/LoadingSpinner';

export default function LegalRouteLoading() {
 return (
  <div className="px-4">
   <LoadingSpinner fullPage message="Loading" />
  </div>
 );
}
