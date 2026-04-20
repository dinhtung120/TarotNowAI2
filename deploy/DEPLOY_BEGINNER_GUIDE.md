# Hướng Dẫn Deploy TarotNow Cho Người Mới (Mô Hình 3 EC2 + Cloudflare)

## 1) Mục tiêu của tài liệu này

Tài liệu này dành cho người chưa biết code, mục tiêu là làm theo từng bước để đưa dự án lên môi trường chạy thật.

Mô hình triển khai mục tiêu:
- `EC2-FE`: chạy `frontend` + `reverse-proxy (nginx)`.
- `EC2-BE`: chạy `backend`.
- `EC2-DB`: chạy `postgres` + `mongodb` + `redis`.
- `Cloudflare Proxy`: đứng trước `EC2-FE` để public website bằng domain HTTPS.

Luồng tổng quan:
`User -> Cloudflare -> EC2-FE (Nginx) -> EC2-BE (API) -> EC2-DB (Postgres/Mongo/Redis)`

## 2) Bạn cần chuẩn bị gì trước khi bắt đầu

## 2.1 Tài khoản và thông tin bắt buộc
- 1 tài khoản AWS có quyền tạo EC2, Security Group.
- 1 tài khoản Cloudflare.
- 1 domain đã quản lý DNS trong Cloudflare.
- SSH keypair để đăng nhập EC2.

## 2.2 Ứng dụng cần có trên máy local (macOS/Linux)
- Terminal.
- `ssh`.
- `git`.

Kiểm tra nhanh:

```bash
ssh -V
git --version
```

## 2.3 Khuyến nghị cấu hình 3 EC2
- OS: Ubuntu Server 24.04 LTS (hoặc 22.04 LTS).
- FE: `t3.small` (hoặc cao hơn khi traffic lớn).
- BE: `t3.medium` (hoặc cao hơn).
- DB: `t3.medium` trở lên, disk đủ lớn (khuyên từ 60GB+).

## 2.4 Security Group (rất quan trọng)

Tạo 3 Security Group:

1. `sg-fe`
- Inbound:
  - `22` từ IP của bạn (ví dụ `1.2.3.4/32`).
  - `80` từ `0.0.0.0/0`.
- Outbound: mặc định `All traffic`.

2. `sg-be`
- Inbound:
  - `22` từ IP của bạn.
  - `5037` chỉ từ `sg-fe` (không mở public).
- Outbound: mặc định `All traffic`.

3. `sg-db`
- Inbound:
  - `22` từ IP của bạn.
  - `5432` chỉ từ `sg-be`.
  - `27017` chỉ từ `sg-be`.
  - `6379` chỉ từ `sg-be`.
- Outbound: mặc định `All traffic`.

Lưu ý:
- Không mở `5432/27017/6379` ra internet.
- Nếu bạn chưa quen, giữ cả 3 máy trong cùng 1 VPC để private IP truy cập được nhau.

## 3) Cài Docker trên cả 3 EC2

Làm bước này trên **từng máy**: FE, BE, DB.

Đăng nhập SSH:

```bash
ssh -i /duong-dan/key.pem ubuntu@<PUBLIC_IP_MAY_DO>
```

Cài Docker Engine + Compose plugin:

```bash
sudo apt-get update
sudo apt-get install -y ca-certificates curl gnupg git

sudo install -m 0755 -d /etc/apt/keyrings
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /etc/apt/keyrings/docker.gpg
sudo chmod a+r /etc/apt/keyrings/docker.gpg

echo \
  "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/ubuntu \
  $(. /etc/os-release && echo $VERSION_CODENAME) stable" | \
  sudo tee /etc/apt/sources.list.d/docker.list > /dev/null

sudo apt-get update
sudo apt-get install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin

sudo usermod -aG docker $USER
newgrp docker

docker --version
docker compose version
```

## 4) Clone dự án trên cả 3 EC2

Trên mỗi máy (FE, BE, DB):

```bash
git clone <URL_REPO_CUA_BAN> TarotNowAI2
cd TarotNowAI2
```

## 5) Tạo file cấu hình production `.env`

Trên mỗi máy:

```bash
cp .env.example .env
```

Mở file để sửa:

```bash
nano .env
```

## 5.1 Giải thích toàn bộ biến trong `.env.example`

| Biến | Ý nghĩa | Ví dụ cho mô hình 3 EC2 |
|---|---|---|
| `PUBLIC_HTTP_PORT` | Cổng public của nginx trên FE | `80` |
| `PUBLIC_BASE_URL` | URL public chính của web | `https://app.example.com` |
| `NEXT_PUBLIC_API_URL` | URL API mà frontend gọi | `https://app.example.com/api/v1` |
| `NEXT_ALLOWED_DEV_HOSTS` | Host bổ sung cho dev | để trống production |
| `PRIVATE_NETWORK_SUBNET` | Mạng nội bộ Docker mỗi máy | `172.30.0.0/24` |
| `POSTGRES_USER` | User PostgreSQL | `tarot_user` |
| `POSTGRES_PASSWORD` | Password PostgreSQL | chuỗi mạnh |
| `POSTGRES_DB` | Tên DB PostgreSQL | `tarotweb` |
| `MONGO_DB` | Tên DB MongoDB | `tarotweb` |
| `REDIS_INSTANCE_NAME` | Prefix key Redis | `TarotNow:` |
| `JWT_SECRETKEY` | Secret ký JWT | chuỗi >= 32 ký tự |
| `JWT_ISSUER` | Issuer JWT | `TarotNowAI` |
| `JWT_AUDIENCE` | Audience JWT | `TarotNowAIUsers` |
| `JWT_EXPIRYMINUTES` | Hạn access token | `60` |
| `JWT_REFRESHEXPIRYDAYS` | Hạn refresh token | `7` |
| `PAYMENT_WEBHOOK_SECRET` | Secret xác thực webhook thanh toán | chuỗi mạnh |
| `PAYOS_CLIENT_ID` | Client ID từ PayOS merchant | UUID/chuỗi PayOS |
| `PAYOS_API_KEY` | API key gọi create payment link | key từ [my.payos.vn](https://my.payos.vn) |
| `PAYOS_CHECKSUM_KEY` | Checksum key verify webhook/signature | key từ [my.payos.vn](https://my.payos.vn) |
| `PAYOS_PARTNER_CODE` | Partner code (nếu tài khoản yêu cầu) | để trống hoặc giá trị PayOS cung cấp |
| `PAYOS_RETURN_URL` | URL frontend nhận kết quả thanh toán thành công | `https://app.example.com/vi/wallet/deposit?payment=return` |
| `PAYOS_CANCEL_URL` | URL frontend khi người dùng hủy thanh toán | `https://app.example.com/vi/wallet/deposit?payment=cancel` |
| `DEPOSIT_LINK_EXPIRY_MINUTES` | Thời gian hết hạn payment link | `15` |
| `MFA_ENCRYPTION_KEY` | Key mã hóa MFA | chuỗi mạnh |
| `SMTP_HOST` | SMTP host | `smtp.gmail.com` |
| `SMTP_PORT` | SMTP port | `587` |
| `SMTP_USERNAME` | SMTP username | tài khoản SMTP |
| `SMTP_PASSWORD` | SMTP password | mật khẩu/app password |
| `SMTP_SENDER_EMAIL` | Email gửi mail | `noreply@example.com` |
| `SMTP_SENDER_NAME` | Tên người gửi mail | `TarotNow Security` |
| `AI_API_KEY` | API key AI provider | key thật |
| `AI_BASE_URL` | Base URL AI provider | `https://api.openai.com/v1/` |
| `AI_MODEL` | Model AI | `gpt-4.1-mini` (hoặc model bạn dùng) |
| `AI_TIMEOUT_SECONDS` | Timeout gọi AI | `30` |
| `AI_MAX_RETRIES` | Số lần retry AI | `2` |
| `FORWARDED_NETWORK_0` | Mạng proxy tin cậy cho BE | VPC CIDR, ví dụ `10.0.0.0/16` |

Lưu ý:
- `FORWARDED_NETWORK_0` nên là CIDR của VPC chứa FE/BE (không dùng IP public).
- Dùng cùng 1 nội dung `.env` trên cả 3 máy để tránh lệch cấu hình.
- Tuyệt đối không commit file này lên git.

## 6) Deploy EC2-DB (Mongo/Postgres/Redis)

Đăng nhập máy DB:

```bash
ssh -i /duong-dan/key.pem ubuntu@<DB_PUBLIC_IP>
cd ~/TarotNowAI2
```

Khởi tạo DB + migrate + seed:

```bash
./deploy/scripts/bootstrap-db.sh .env docker-compose.prod.yml
```

Kiểm tra container:

```bash
docker compose -f docker-compose.prod.yml ps
```

Lấy private IP của DB (dùng cho BE):

```bash
hostname -I | awk '{print $1}'
```

Ghi lại giá trị này, ví dụ: `10.0.3.21`.

## 7) Deploy EC2-BE (Backend)

Đăng nhập máy BE:

```bash
ssh -i /duong-dan/key.pem ubuntu@<BE_PUBLIC_IP>
cd ~/TarotNowAI2
```

Lấy private IP BE (dùng cho FE):

```bash
hostname -I | awk '{print $1}'
```

Tạo file override cho BE:

```bash
cat > deploy/docker-compose.3ec2.backend.override.yml <<'YAML'
services:
  backend:
    ports:
      - "5037:5037"
    environment:
      CONNECTIONSTRINGS__POSTGRESQL: Host=<DB_PRIVATE_IP>;Port=5432;Database=${POSTGRES_DB};Username=${POSTGRES_USER};Password=${POSTGRES_PASSWORD}
      CONNECTIONSTRINGS__MONGODB: mongodb://<DB_PRIVATE_IP>:27017/${MONGO_DB}
      CONNECTIONSTRINGS__REDIS: <DB_PRIVATE_IP>:6379
YAML
```

Thay `<DB_PRIVATE_IP>` bằng IP private thật của máy DB.

Chạy backend (không kéo theo DB local):

```bash
docker compose \
  -f docker-compose.prod.yml \
  -f deploy/docker-compose.3ec2.backend.override.yml \
  up -d --build --no-deps backend
```

Kiểm tra BE local:

```bash
curl -fsS http://localhost:5037/api/v1/health/live
curl -fsS http://localhost:5037/api/v1/health/ready
```

Nếu `ready` trả lỗi, kiểm tra:

```bash
docker compose -f docker-compose.prod.yml logs --no-color backend
```

## 8) Deploy EC2-FE (Frontend + Nginx reverse proxy)

Đăng nhập máy FE:

```bash
ssh -i /duong-dan/key.pem ubuntu@<FE_PUBLIC_IP>
cd ~/TarotNowAI2
```

Sửa upstream API trong nginx để trỏ sang BE private IP:

```bash
cp deploy/nginx/conf.d/default.conf deploy/nginx/conf.d/default.conf.bak
nano deploy/nginx/conf.d/default.conf
```

Tìm dòng:

```nginx
set $backend_upstream http://backend:5037;
```

Đổi thành:

```nginx
set $backend_upstream http://<BE_PRIVATE_IP>:5037;
```

Lưu file.

Chạy FE + nginx (không kéo BE local):

```bash
docker compose \
  -f docker-compose.prod.yml \
  up -d --build --no-deps frontend reverse-proxy
```

Kiểm tra trên FE:

```bash
./deploy/scripts/smoke.sh http://localhost
```

Kiểm tra từ máy local:

```bash
curl -I http://<FE_PUBLIC_IP>
```

## 9) Cấu hình Cloudflare Proxy (Domain + HTTPS)

Trong Cloudflare:

1. DNS
- Tạo record `A`:
  - Name: `app` (hoặc root `@` tùy domain bạn dùng).
  - IPv4: `<FE_PUBLIC_IP>`.
  - Proxy status: `Proxied` (mây cam).

2. SSL/TLS
- Vì origin hiện tại chạy HTTP cổng 80, cách đơn giản nhất là dùng mode tương thích với HTTP origin.
- Chọn SSL/TLS mode: `Flexible` (dễ triển khai nhất với cấu hình hiện tại).
- Bật chuyển hướng HTTPS cho người dùng ở edge Cloudflare (Always Use HTTPS).

3. Test domain
- Truy cập `https://app.example.com`.
- Test API qua FE proxy:

```bash
curl -fsS https://app.example.com/api/v1/health/live
curl -fsS https://app.example.com/api/v1/health/ready
```

## 9.1 Cảnh báo bảo mật và đường nâng cấp “chuẩn cứng”

Mô hình mặc định trên là HTTPS ở Cloudflare edge, nhưng đoạn Cloudflare -> FE origin có thể chưa phải TLS end-to-end.

Để nâng cấp chuẩn hơn (end-to-end TLS):
- Tạo Cloudflare Origin Certificate.
- Cài cert lên FE.
- Sửa các file sau:
  - `deploy/nginx/conf.d/default.conf` (thêm `listen 443 ssl`, cấu hình cert/key, redirect 80 -> 443).
  - `deploy/nginx/nginx.conf` (điều chỉnh nếu cần policy TLS/log).
  - `docker-compose.prod.yml` (mở thêm cổng `443:443`, mount thư mục cert vào container nginx).
- Chuyển Cloudflare SSL mode sang `Full (strict)`.

## 10) Vận hành sau deploy

## 10.1 Kiểm tra trạng thái nhanh

Trên FE:

```bash
docker compose -f docker-compose.prod.yml ps
docker compose -f docker-compose.prod.yml logs --no-color reverse-proxy frontend
```

Trên BE:

```bash
docker compose -f docker-compose.prod.yml logs --no-color backend
```

Trên DB:

```bash
docker compose -f docker-compose.prod.yml logs --no-color postgres mongodb redis
```

## 10.2 Quy trình update phiên bản

Thực hiện theo thứ tự: DB (nếu cần migration) -> BE -> FE.

Trên từng máy tương ứng:

```bash
cd ~/TarotNowAI2
git pull
```

- DB (nếu có thay đổi DB):

```bash
./deploy/scripts/bootstrap-db.sh .env docker-compose.prod.yml
```

- BE:

```bash
docker compose \
  -f docker-compose.prod.yml \
  -f deploy/docker-compose.3ec2.backend.override.yml \
  up -d --build --no-deps backend
```

- FE:

```bash
docker compose -f docker-compose.prod.yml up -d --build --no-deps frontend reverse-proxy
```

## 10.3 Backup/Restore (chạy trên DB host)

Backup:

```bash
ENV_FILE=.env ./deploy/scripts/backup-db.sh
```

Restore:

```bash
ENV_FILE=.env ./deploy/scripts/restore-db.sh backups/<timestamp>
```

## 10.4 Về các drill script

Các script dưới đây được thiết kế chủ yếu cho mô hình single-host:
- `deploy/scripts/failure-drills.sh`
- `deploy/scripts/scale-drill.sh`
- `deploy/scripts/rollback-drill.sh`

Với mô hình 3 EC2, bạn nên thực hiện drill thủ công theo từng lớp (FE/BE/DB), ví dụ:
- Tắt BE container rồi xác nhận FE trả lỗi API đúng kỳ vọng.
- Restart Redis ở DB host rồi kiểm tra `ready` trên API.
- Deploy bản mới trên BE rồi rollback bằng image/tag cũ.

## 11) Checklist trước Go-live

- [ ] `https://app.example.com` mở được.
- [ ] `https://app.example.com/api/v1/health/live` trả 200.
- [ ] `https://app.example.com/api/v1/health/ready` trả 200.
- [ ] Đăng nhập/đọc dữ liệu/upload media hoạt động.
- [ ] Đã chạy backup ít nhất 1 lần và thấy file backup.
- [ ] Security Group không mở dư cổng DB ra internet.
- [ ] File `.env` không nằm trong git.

## 12) Checklist sau Go-live 24 giờ

- [ ] Kiểm tra CPU/RAM/disk cả 3 EC2.
- [ ] Kiểm tra log lỗi backend/nginx.
- [ ] Kiểm tra tăng trưởng dung lượng DB.
- [ ] Chạy thêm 1 bản backup.
- [ ] Kiểm tra Cloudflare analytics có lỗi bất thường không.

## 13) Troubleshooting chi tiết

Mẫu đọc nhanh mỗi lỗi:
- Dấu hiệu
- Nguyên nhân
- Cách xử lý
- Cách xác nhận đã hết

## 13.1 Lỗi thiếu biến môi trường (`is required`)

Dấu hiệu:
- `docker compose config` hoặc `up` báo: `... is required`.

Nguyên nhân:
- Thiếu biến trong `.env` hoặc sai tên biến.

Cách xử lý:
1. Mở file env:
   ```bash
   nano .env
   ```
2. Đối chiếu từng biến với `.env.example`.
3. Lưu file và chạy lại:
   ```bash
   docker compose -f docker-compose.prod.yml config
   ```

Cách xác nhận đã hết:
- Lệnh `config` chạy không lỗi.

## 13.2 Lỗi health `503` ở `/api/v1/health/ready`

Dấu hiệu:
- `curl .../health/ready` trả `503`.

Nguyên nhân:
- Backend không kết nối được Postgres/Mongo/Redis.
- Sai `DB_PRIVATE_IP`, sai mật khẩu DB, hoặc SG chặn cổng.

Cách xử lý:
1. Trên BE kiểm tra log backend:
   ```bash
   docker compose -f docker-compose.prod.yml logs --no-color backend
   ```
2. Kiểm tra lại file override BE có đúng IP DB.
3. Kiểm tra SG của DB mở `5432/27017/6379` cho `sg-be`.
4. Thử kết nối TCP từ BE sang DB:
   ```bash
   nc -zv <DB_PRIVATE_IP> 5432
   nc -zv <DB_PRIVATE_IP> 27017
   nc -zv <DB_PRIVATE_IP> 6379
   ```

Cách xác nhận đã hết:
- `curl -fsS http://localhost:5037/api/v1/health/ready` trên BE trả 200.

## 13.3 Lỗi port đã được sử dụng

Dấu hiệu:
- Báo `address already in use` khi start container.

Nguyên nhân:
- Port 80 (FE) hoặc 5037 (BE) đang bị dịch vụ khác chiếm.

Cách xử lý:
1. Kiểm tra port:
   ```bash
   sudo ss -ltnp | grep -E ':80|:5037'
   ```
2. Tắt dịch vụ đang chiếm port hoặc đổi port mapping.
3. Chạy lại `docker compose up`.

Cách xác nhận đã hết:
- `docker compose ps` hiển thị container `Up`.

## 13.4 Lỗi Docker daemon / quyền docker

Dấu hiệu:
- `Cannot connect to the Docker daemon`.
- `permission denied while trying to connect to the Docker daemon socket`.

Nguyên nhân:
- Docker chưa chạy hoặc user chưa vào group docker.

Cách xử lý:
1. Start Docker:
   ```bash
   sudo systemctl enable --now docker
   ```
2. Add user vào group docker:
   ```bash
   sudo usermod -aG docker $USER
   newgrp docker
   ```
3. Kiểm tra:
   ```bash
   docker ps
   ```

Cách xác nhận đã hết:
- `docker ps` chạy được không cần `sudo`.

## 13.5 Lỗi Cloudflare `522` và `525`

Dấu hiệu:
- Truy cập domain thấy lỗi `522` hoặc `525`.

Nguyên nhân:
- `522`: Cloudflare không kết nối được tới origin (FE không chạy, SG chặn port 80, IP DNS sai).
- `525`: Lỗi handshake TLS giữa Cloudflare và origin (thường do mode SSL không khớp cấu hình TLS origin).

Cách xử lý:
1. Kiểm tra DNS A record trỏ đúng FE public IP.
2. Kiểm tra FE mở port 80 public và nginx chạy:
   ```bash
   curl -fsS http://localhost/nginx-health
   ```
3. Với `525`: kiểm tra lại SSL mode Cloudflare so với trạng thái TLS thực của origin.

Cách xác nhận đã hết:
- `curl -I https://app.example.com` trả mã 2xx/3xx.

## 13.6 Lỗi FE build do `NODE_TLS_REJECT_UNAUTHORIZED=0`

Dấu hiệu:
- Build frontend báo lỗi: `NODE_TLS_REJECT_UNAUTHORIZED=0 is not allowed in production`.

Nguyên nhân:
- Bạn đang set biến này trong môi trường build.

Cách xử lý:
1. Mở file env frontend (nếu có):
   ```bash
   nano Frontend/.env.local
   ```
2. Xóa dòng `NODE_TLS_REJECT_UNAUTHORIZED=0`.
3. Build lại FE.

Cách xác nhận đã hết:
- `docker compose ... up -d --build frontend ...` chạy thành công.

## 13.7 FE trả 502 khi gọi API

Dấu hiệu:
- Web mở được nhưng gọi `/api/...` bị `502 Bad Gateway`.

Nguyên nhân:
- FE nginx chưa trỏ đúng `BE_PRIVATE_IP`, hoặc SG `sg-be` chưa cho phép từ `sg-fe` vào port 5037.

Cách xử lý:
1. Kiểm tra line upstream trong `deploy/nginx/conf.d/default.conf` trên FE.
2. Kiểm tra BE có mở port 5037:
   ```bash
   sudo ss -ltnp | grep 5037
   ```
3. Kiểm tra SG của BE.

Cách xác nhận đã hết:
- `curl -fsS http://localhost/api/v1/health/live` trên FE trả 200.

## 14) Bảo mật tối thiểu bắt buộc

- Không commit `.env` vào git.
- Chỉ dùng SSH key, không dùng password login.
- Giới hạn inbound SSH theo IP cố định của bạn.
- Không public cổng DB (`5432`, `27017`, `6379`).
- Xoay vòng secrets định kỳ (`JWT_SECRETKEY`, SMTP, AI key, webhook secret).
- Luôn backup trước các thay đổi lớn.

Lưu ý giới hạn kiến trúc hiện tại:
- DB vẫn chạy container trên 1 EC2, chưa phải managed service HA.
- Đây là mô hình thực dụng để go-live nhanh; khi traffic lớn nên nâng cấp dần sang RDS/ElastiCache/DocumentDB.

## 15) File nào cần sửa trong mô hình 3 EC2

Bắt buộc:
- `.env` (trên cả 3 máy).
- `deploy/docker-compose.3ec2.backend.override.yml` (tạo mới trên BE).
- `deploy/nginx/conf.d/default.conf` (trên FE, sửa `backend_upstream` trỏ BE private IP).

Khi nâng cấp end-to-end TLS:
- `deploy/nginx/conf.d/default.conf`
- `deploy/nginx/nginx.conf`
- `docker-compose.prod.yml`

---

Nếu bạn làm đúng tuần tự tài liệu này, bạn sẽ triển khai được mô hình 3 EC2 cho dự án hiện tại mà không cần biết sâu về code.
