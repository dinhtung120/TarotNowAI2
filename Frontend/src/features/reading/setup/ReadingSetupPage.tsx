"use client";

import { useEffect, useRef } from "react";
import { Compass, Sparkles } from "lucide-react";
import { useReadingSetupPage } from "@/features/reading/setup/useReadingSetupPage";
import { Input, SectionHeader } from "@/shared/ui";
import {
 ReadingCurrencySelector,
 ReadingSetupSubmitAction,
 ReadingSpreadsGrid,
} from "@/features/reading/setup/components";
import { cn } from "@/lib/utils";

/**
 * Trang thiết lập phiên trải bài Tarot.
 * Cho phép người dùng chọn kiểu trải bài, loại tiền tệ và nhập câu hỏi.
 */
export default function ReadingSetupPage() {
	/* 
	   Sử dụng ViewModel hook để quản lý trạng thái và logic của trang.
	   vm chứa các state như selectedSpread, selectedCurrency và các hàm điều hướng.
	*/
	const vm = useReadingSetupPage();
	const submitHandler = vm.handleSubmit(vm.submitSetup);

	/* 
	   Tạo một tham chiếu (ref) đến vùng chứa nút "Bắt Đầu Giải Mã".
	   Mục đích là để có thể cuộn trang đến vị trí này một cách tự động.
	*/
	const submitActionRef = useRef<HTMLDivElement>(null);
	/* 
	   Dùng ref này để lưu trữ giá trị spread đã chọn trước đó, 
	   giúp tránh việc tự động cuộn ngay khi trang vừa tải lần đầu (mount).
	*/
	const prevSpreadRef = useRef<string>(vm.selectedSpread);

	/* 
	   Lắng nghe sự thay đổi của selectedSpread để thực hiện cuộn trang trên Mobile.
	*/
	useEffect(() => {
		// Kiểm tra xem đây có phải là lần thay đổi thực sự từ phía người dùng hay không
		// và đảm bảo mã chỉ chạy trên trình duyệt (window defined).
		if (typeof window !== "undefined" && vm.selectedSpread !== prevSpreadRef.current) {
			// Chỉ thực hiện cuộn nếu chiều rộng màn hình nhỏ hơn 768px (Mobile/Tablet nhỏ).
			// Điều này giúp tránh gây khó chịu cho người dùng Desktop khi trang đã hiển thị đầy đủ.
			if (window.innerWidth < 768) {
				submitActionRef.current?.scrollIntoView({
					behavior: "smooth", // Cuộn mượt mà để tăng trải nghiệm người dùng.
					block: "center", // Cuộn sao cho phần tử nằm ở giữa màn hình để dễ quan sát nút bấm.
				});
			}
		}
		// Cập nhật lại giá trị spread hiện tại vào ref để so sánh ở lần thay đổi tiếp theo.
		prevSpreadRef.current = vm.selectedSpread;
	}, [vm.selectedSpread]);

	return (
		<div className={cn("mx-auto", "max-w-4xl", "px-6", "pt-10", "pb-20", "font-sans")}>
			{/* Tiêu đề phần thiết lập */}
			<SectionHeader title={vm.t("title")} subtitle={vm.t("subtitle")} tag={vm.t("tag")} tagIcon={<Compass className={cn("h-3", "w-3")} />} className={cn("mb-10")} />

			{/* Hiển thị thông báo lỗi nếu khởi tạo không thành công */}
			{vm.initError ? <div className={cn("mb-8", "rounded-xl", "border", "border-red-500/20", "bg-red-500/10", "p-4", "text-sm", "text-red-400")}>{vm.initError}</div> : null}



			{/* Bộ chọn loại tiền tệ thanh toán (Vàng/Kim cương) */}
			<ReadingCurrencySelector selectedCurrency={vm.selectedCurrency} labels={{ title: vm.t("select_currency"), gold: vm.t("currency_gold"), diamond: vm.t("currency_diamond") }} onSelectCurrency={vm.setSelectedCurrency} />

			<form onSubmit={submitHandler} className={cn("space-y-10")}>
				{/* Lưới các kiểu trải bài để người dùng lựa chọn */}
				<ReadingSpreadsGrid spreads={vm.spreads} selectedSpread={vm.selectedSpread} selectedCurrency={vm.selectedCurrency} expBonusLabel={(amount) => vm.t("exp_bonus", { amount })} onSelectSpread={vm.setSelectedSpread} />

				{/* 
				   Cánh cửa nhập câu hỏi: Chỉ hiển thị khi kiểu trải bài không phải là "Nhật Ấn (1 Lá)".
				   Vì xem bài hàng ngày thường mang tính chất thông điệp chung.
				*/}
				{vm.selectedSpread !== "daily_1" ? (
					<Input label={vm.t("question_label")} isTextarea placeholder={vm.t("question_placeholder")} leftIcon={<Sparkles className={cn("h-5", "w-5", "text-violet-400")} />} error={vm.errors.question?.message} {...vm.register("question")} />
				) : null}

				{/* Vùng chứa nút hành động cuối cùng, được gắn ref để thực hiện auto-scroll */}
				<div ref={submitActionRef}>
					<ReadingSetupSubmitAction isInitializing={vm.isInitializing} preparingLabel={vm.t("preparing")} submitLabel={vm.t("cta_draw")} />
				</div>
			</form>
		</div>
	);
}
