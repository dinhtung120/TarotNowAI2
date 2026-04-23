# SYSTEM BALANCE REVIEW

## 1) Kết luận nhanh
Hệ thống hiện **đã vận hành được** nhưng cân bằng economy chưa tối ưu ở 4 điểm chính:
- Giá `spread_10` đang dốc mạnh so với `spread_3`/`spread_5`
- Follow-up tier tăng quá gắt ở lượt cuối
- Phí rút 10% cao, dễ tạo cảm giác hụt cho reader
- Daily check-in reward thấp so với giá tiêu hao core loop

## 2) Dữ liệu nền hiện tại (default)
- Quy đổi: `1 diamond = 100 VND`
- Deposit package: `50,000 VND -> 500 diamond` (và các gói cùng tỷ lệ)
- Reading pricing:
  - `pricing.spread_3.gold = 50`
  - `pricing.spread_5.gold = 100`
  - `pricing.spread_10.gold = 500`
  - `pricing.spread_3.diamond = 5`
  - `pricing.spread_5.diamond = 10`
  - `pricing.spread_10.diamond = 50`
- Follow-up: `followup.price_tiers = [1,2,4,8,16]`
- Withdrawal:
  - `withdrawal.min_diamond = 500`
  - `withdrawal.fee_rate = 0.10`
- Check-in: `checkin.daily_gold = 5`

## 3) Đề xuất tuning cân bằng (ưu tiên)

### Priority A (nên áp dụng sớm)
1. Làm mượt giá `spread_10`
- Hiện tại: `spread_10` = 10x `spread_3`
- Đề xuất:
  - `pricing.spread_10.gold: 500 -> 300`
  - `pricing.spread_10.diamond: 50 -> 30`

2. Giảm độ dốc follow-up ở lượt cuối
- Hiện tại: `[1,2,4,8,16]`
- Đề xuất: `[1,2,3,5,8]`
- Mục tiêu: giảm cảm giác “phí nhảy sốc” ở user trung bình.

3. Giảm nhẹ phí rút để tăng giữ chân reader
- Hiện tại: `withdrawal.fee_rate = 0.10`
- Đề xuất: `0.08`

### Priority B (A/B test 2-4 tuần)
4. Tăng thưởng check-in để cải thiện retention đầu ngày
- Hiện tại: `checkin.daily_gold = 5`
- Đề xuất test: `10` hoặc `15`

5. Tối ưu ngưỡng free follow-up
- Hiện tại:
  - `threshold_low = 6`
  - `threshold_mid = 11`
  - `threshold_high = 16`
- Đề xuất test cohort mới:
  - `5 / 10 / 15`

## 4) Gói cấu hình đề xuất (copy để update nhanh)

### Scalars
- `pricing.spread_10.gold = 300`
- `pricing.spread_10.diamond = 30`
- `withdrawal.fee_rate = 0.08`
- `checkin.daily_gold = 10`
- `followup.free_slots.threshold_low = 5`
- `followup.free_slots.threshold_mid = 10`
- `followup.free_slots.threshold_high = 15`

### JSON
```json
followup.price_tiers = [1,2,3,5,8]
```

## 5) Chỉ số cần theo dõi sau khi chỉnh
- Conversion `reading init -> completed` theo từng spread
- Tỷ lệ mua follow-up lượt 3/4/5
- Retention D1/D7 nhóm có check-in
- Số lượng withdrawal/tuần và tỷ lệ reject
- ARPPU theo cohort trước/sau tuning

## 6) Ghi chú triển khai
- Tất cả đề xuất trên đã có thể chỉnh trực tiếp tại trang admin:
  - `/[locale]/admin/system-configs`
- Nên rollout theo từng nhóm key (pricing trước, withdrawal sau) để dễ đo tác động.

