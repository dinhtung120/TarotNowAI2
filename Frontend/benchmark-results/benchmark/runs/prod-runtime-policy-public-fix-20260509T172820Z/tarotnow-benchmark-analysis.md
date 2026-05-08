# TarotNow Benchmark Analysis

- Run time (UTC): 2026-05-08T17:43:12.201Z
- Base: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Matrix: Chromium desktop + mobile, logged-out + logged-in-admin + logged-in-reader

## Scenario Summary
| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 24.9 | 3082 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 29.5 | 4717 | 21 | 0 | 0 | 15 | 0 | yes |
| logged-in-reader | desktop | 33 | 29.4 | 3248 | 0 | 0 | 0 | 12 | 0 | yes |
| logged-out | mobile | 9 | 24.9 | 3240 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 29.1 | 6671 | 65 | 0 | 0 | 4 | 0 | yes |
| logged-in-reader | mobile | 33 | 30.3 | 3145 | 1 | 0 | 1 | 13 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.6 | 4815 | 0 |
| logged-in-admin | desktop | chat | 1 | 28.0 | 4492 | 0 |
| logged-in-admin | desktop | collection | 1 | 29.0 | 5423 | 1 |
| logged-in-admin | desktop | community | 1 | 37.0 | 7259 | 0 |
| logged-in-admin | desktop | gacha | 2 | 32.5 | 4139 | 2 |
| logged-in-admin | desktop | gamification | 1 | 30.0 | 4724 | 0 |
| logged-in-admin | desktop | home | 1 | 29.0 | 4973 | 0 |
| logged-in-admin | desktop | inventory | 1 | 32.0 | 3392 | 1 |
| logged-in-admin | desktop | leaderboard | 1 | 31.0 | 6130 | 0 |
| logged-in-admin | desktop | legal | 3 | 25.0 | 4159 | 0 |
| logged-in-admin | desktop | notifications | 1 | 28.0 | 6511 | 1 |
| logged-in-admin | desktop | profile | 3 | 29.0 | 4643 | 13 |
| logged-in-admin | desktop | reader | 1 | 28.0 | 4350 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.6 | 4319 | 2 |
| logged-in-admin | desktop | reading | 5 | 31.8 | 4543 | 0 |
| logged-in-admin | desktop | wallet | 4 | 29.0 | 4946 | 1 |
| logged-in-admin | mobile | admin | 10 | 29.3 | 7130 | 0 |
| logged-in-admin | mobile | chat | 1 | 28.0 | 6205 | 0 |
| logged-in-admin | mobile | collection | 1 | 29.0 | 7702 | 13 |
| logged-in-admin | mobile | community | 1 | 34.0 | 9618 | 0 |
| logged-in-admin | mobile | gacha | 2 | 34.5 | 5060 | 1 |
| logged-in-admin | mobile | gamification | 1 | 29.0 | 7358 | 0 |
| logged-in-admin | mobile | home | 1 | 35.0 | 3965 | 0 |
| logged-in-admin | mobile | inventory | 1 | 35.0 | 4040 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 30.0 | 5420 | 1 |
| logged-in-admin | mobile | legal | 3 | 25.0 | 6385 | 0 |
| logged-in-admin | mobile | notifications | 1 | 28.0 | 7924 | 0 |
| logged-in-admin | mobile | profile | 3 | 28.3 | 5813 | 22 |
| logged-in-admin | mobile | reader | 1 | 28.0 | 8531 | 0 |
| logged-in-admin | mobile | readers | 7 | 28.0 | 5955 | 1 |
| logged-in-admin | mobile | reading | 5 | 29.2 | 6367 | 3 |
| logged-in-admin | mobile | wallet | 4 | 28.0 | 8643 | 24 |
| logged-in-reader | desktop | chat | 1 | 28.0 | 2778 | 0 |
| logged-in-reader | desktop | collection | 1 | 29.0 | 7088 | 0 |
| logged-in-reader | desktop | community | 1 | 30.0 | 3515 | 0 |
| logged-in-reader | desktop | gacha | 2 | 35.5 | 4057 | 0 |
| logged-in-reader | desktop | gamification | 1 | 29.0 | 2915 | 0 |
| logged-in-reader | desktop | home | 1 | 35.0 | 3099 | 0 |
| logged-in-reader | desktop | inventory | 1 | 35.0 | 3040 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 30.0 | 3022 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.0 | 2929 | 0 |
| logged-in-reader | desktop | notifications | 1 | 29.0 | 2805 | 0 |
| logged-in-reader | desktop | profile | 3 | 29.7 | 2857 | 0 |
| logged-in-reader | desktop | reader | 1 | 34.0 | 6241 | 0 |
| logged-in-reader | desktop | readers | 7 | 28.0 | 2893 | 0 |
| logged-in-reader | desktop | reading | 5 | 28.6 | 2971 | 0 |
| logged-in-reader | desktop | wallet | 4 | 29.5 | 3031 | 0 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 2824 | 0 |
| logged-in-reader | mobile | collection | 1 | 33.0 | 5764 | 0 |
| logged-in-reader | mobile | community | 1 | 37.0 | 3923 | 0 |
| logged-in-reader | mobile | gacha | 2 | 34.5 | 3227 | 0 |
| logged-in-reader | mobile | gamification | 1 | 29.0 | 3240 | 1 |
| logged-in-reader | mobile | home | 1 | 35.0 | 3058 | 0 |
| logged-in-reader | mobile | inventory | 1 | 36.0 | 3206 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 30.0 | 2946 | 0 |
| logged-in-reader | mobile | legal | 3 | 25.0 | 2786 | 0 |
| logged-in-reader | mobile | notifications | 1 | 28.0 | 4535 | 0 |
| logged-in-reader | mobile | profile | 3 | 29.3 | 2935 | 0 |
| logged-in-reader | mobile | reader | 1 | 28.0 | 2823 | 0 |
| logged-in-reader | mobile | readers | 7 | 28.3 | 2891 | 0 |
| logged-in-reader | mobile | reading | 5 | 29.2 | 2911 | 0 |
| logged-in-reader | mobile | wallet | 4 | 35.3 | 3266 | 0 |
| logged-out | desktop | auth | 5 | 24.0 | 2897 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 3969 | 0 |
| logged-out | desktop | legal | 3 | 25.0 | 3095 | 0 |
| logged-out | mobile | auth | 5 | 24.0 | 2795 | 0 |
| logged-out | mobile | home | 1 | 29.0 | 5409 | 0 |
| logged-out | mobile | legal | 3 | 25.0 | 3258 | 0 |

## Key Findings
1. Critical: 7 page(s) vượt 35 requests.
2. High: 142 page(s) vượt 25 requests.
3. High: 1770 request vượt 800ms.
4. Medium: 660 request trong dải 400-800ms.
5. High: phát hiện 1 handshake redirect(s), cần kiểm tra vòng lặp auth/session.
6. Collection-focus: 0 image request(s) >800ms trên 0 image request(s).

## Top Slow Pages
| Scenario | Viewport | Feature | Route | Navigate (ms) | Request count | LCP (ms) | TBT (ms) | CLS |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 9618 | 34 | 5280 | 0.0 | 0.0173 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 9090 | 28 | 1688 | 0.0 | 0.0071 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | 8703 | 29 | 2720 | 0.0 | 0.0000 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 8698 | 28 | 3060 | 0.0 | 0.0071 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 8668 | 28 | - | 0.0 | 0.0000 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | 8531 | 28 | 2812 | 0.0 | 0.0071 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 8283 | 29 | 3948 | 0.0 | 0.0000 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 8114 | 28 | 2020 | 0.0 | 0.0071 |
| logged-in-admin | mobile | reading | /vi/reading/history | 8006 | 28 | 1728 | 0.0 | 0.0071 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 7924 | 28 | 1860 | 0.0 | 0.0088 |

## High Slow Requests (> 800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 6510 | 2878 | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 6224 | 900 | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 6216 | 355 | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | GET | 200 | 6087 | 2463 | https://www.tarotnow.xyz/_next/static/chunks/02u6sgqed9rhs.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 5446 | 354 | https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-admin | mobile | reading | /vi/reading/history | GET | 200 | 5381 | 365 | https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-admin | mobile | reading | /vi/reading/history | GET | 200 | 5374 | 402 | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 5331 | 832 | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 5326 | 525 | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 5326 | 778 | https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 5320 | 344 | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 5313 | 575 | https://www.tarotnow.xyz/_next/static/chunks/0pwly2ey-ozy~.js |
| logged-in-admin | mobile | reading | /vi/reading/history | GET | 200 | 5268 | 355 | https://www.tarotnow.xyz/_next/static/chunks/0pwly2ey-ozy~.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 5176 | 345 | https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 5164 | 411 | https://www.tarotnow.xyz/_next/static/chunks/0pwly2ey-ozy~.js |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 800 | 46 | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 799 | 470 | https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 799 | 796 | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 799 | 354 | https://www.tarotnow.xyz/_next/static/chunks/07or0in3dm_w_.js |
| logged-in-reader | mobile | reading | /vi/reading/session/9eb4ef2a-a6bd-4f2a-a880-24f3129f529a | GET | 200 | 799 | 314 | https://www.tarotnow.xyz/vi/reading/session/9eb4ef2a-a6bd-4f2a-a880-24f3129f529a |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 798 | 785 | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 797 | 557 | https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 797 | 325 | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | mobile | reading | /vi/reading/session/80b5e443-990c-4191-929f-6efbaa05985e | GET | 200 | 797 | 317 | https://www.tarotnow.xyz/vi/reading/session/80b5e443-990c-4191-929f-6efbaa05985e |
| logged-in-admin | desktop | admin | /vi/admin/promotions | GET | 200 | 796 | 771 | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-admin | desktop | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 795 | 180 | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 795 | 328 | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | GET | 200 | 794 | 321 | https://www.tarotnow.xyz/vi/notifications |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 793 | 334 | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-admin | desktop | admin | /vi/admin/disputes | GET | 200 | 792 | 164 | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |

## Duplicate Request Candidates (Non-telemetry)
| Scenario | Viewport | Feature | Route | Count | Request Key |
| --- | --- | --- | --- | ---: | --- |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 2 | POST https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/07or0in3dm_w_.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |

## Collection Image Metrics
| Scenario | Viewport | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | 0 | 0 | 0 | 0 | 0 | 0 |
| logged-in-reader | desktop | 0 | 0 | 0 | 0 | 0 | 0 |
| logged-in-admin | mobile | 0 | 0 | 0 | 0 | 0 | 0 |
| logged-in-reader | mobile | 0 | 0 | 0 | 0 | 0 | 0 |

## Notes
- Duplicate `/cdn-cgi/rum` được xem là telemetry của Cloudflare, không coi là business over-fetch.
- Coverage dynamic route có thể bị `coverage-blocked` nếu environment không có dữ liệu phù hợp tại thời điểm chạy.