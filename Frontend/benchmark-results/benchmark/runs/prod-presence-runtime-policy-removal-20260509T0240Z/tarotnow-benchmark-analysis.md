# TarotNow Benchmark Analysis

- Run time (UTC): 2026-05-08T19:44:52.162Z
- Base: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Matrix: Chromium desktop + mobile, logged-out + logged-in-admin + logged-in-reader

## Scenario Summary
| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 24.9 | 2853 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 29.8 | 2982 | 0 | 0 | 1 | 17 | 0 | yes |
| logged-in-reader | desktop | 33 | 29.6 | 2966 | 0 | 0 | 1 | 16 | 1 | yes |
| logged-out | mobile | 9 | 25.1 | 2762 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 29.2 | 2890 | 0 | 0 | 0 | 14 | 0 | yes |
| logged-in-reader | mobile | 33 | 29.8 | 2932 | 0 | 0 | 1 | 17 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.3 | 2897 | 0 |
| logged-in-admin | desktop | chat | 1 | 28.0 | 2701 | 0 |
| logged-in-admin | desktop | collection | 1 | 29.0 | 6171 | 0 |
| logged-in-admin | desktop | community | 1 | 35.0 | 4118 | 0 |
| logged-in-admin | desktop | gacha | 2 | 33.0 | 2904 | 0 |
| logged-in-admin | desktop | gamification | 1 | 30.0 | 3297 | 0 |
| logged-in-admin | desktop | home | 1 | 26.0 | 2870 | 0 |
| logged-in-admin | desktop | inventory | 1 | 32.0 | 2879 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 31.0 | 3228 | 0 |
| logged-in-admin | desktop | legal | 3 | 25.0 | 2707 | 0 |
| logged-in-admin | desktop | notifications | 1 | 28.0 | 2701 | 0 |
| logged-in-admin | desktop | profile | 3 | 29.3 | 2817 | 0 |
| logged-in-admin | desktop | reader | 1 | 28.0 | 2724 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.3 | 2803 | 0 |
| logged-in-admin | desktop | reading | 5 | 31.0 | 2840 | 0 |
| logged-in-admin | desktop | wallet | 4 | 35.3 | 3092 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.2 | 2764 | 0 |
| logged-in-admin | mobile | chat | 1 | 28.0 | 2760 | 0 |
| logged-in-admin | mobile | collection | 1 | 29.0 | 5490 | 0 |
| logged-in-admin | mobile | community | 1 | 36.0 | 3670 | 0 |
| logged-in-admin | mobile | gacha | 2 | 32.0 | 2867 | 0 |
| logged-in-admin | mobile | gamification | 1 | 32.0 | 2896 | 0 |
| logged-in-admin | mobile | home | 1 | 26.0 | 2642 | 0 |
| logged-in-admin | mobile | inventory | 1 | 32.0 | 2847 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 32.0 | 2886 | 0 |
| logged-in-admin | mobile | legal | 3 | 25.0 | 2661 | 0 |
| logged-in-admin | mobile | notifications | 1 | 28.0 | 2734 | 0 |
| logged-in-admin | mobile | profile | 3 | 29.3 | 3080 | 0 |
| logged-in-admin | mobile | reader | 1 | 28.0 | 2732 | 0 |
| logged-in-admin | mobile | readers | 7 | 28.3 | 2777 | 0 |
| logged-in-admin | mobile | reading | 5 | 30.4 | 2920 | 0 |
| logged-in-admin | mobile | wallet | 4 | 28.5 | 2740 | 0 |
| logged-in-reader | desktop | chat | 1 | 28.0 | 2840 | 0 |
| logged-in-reader | desktop | collection | 1 | 29.0 | 6259 | 0 |
| logged-in-reader | desktop | community | 1 | 30.0 | 3486 | 0 |
| logged-in-reader | desktop | gacha | 2 | 31.5 | 2859 | 0 |
| logged-in-reader | desktop | gamification | 1 | 31.0 | 2910 | 0 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2727 | 0 |
| logged-in-reader | desktop | inventory | 1 | 32.0 | 2880 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 33.0 | 2876 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.0 | 2783 | 0 |
| logged-in-reader | desktop | notifications | 1 | 30.0 | 2875 | 0 |
| logged-in-reader | desktop | profile | 3 | 29.7 | 2886 | 0 |
| logged-in-reader | desktop | reader | 1 | 28.0 | 2699 | 0 |
| logged-in-reader | desktop | readers | 7 | 28.4 | 2783 | 0 |
| logged-in-reader | desktop | reading | 5 | 28.6 | 2774 | 0 |
| logged-in-reader | desktop | wallet | 4 | 35.5 | 3063 | 0 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 2707 | 0 |
| logged-in-reader | mobile | collection | 1 | 29.0 | 5560 | 0 |
| logged-in-reader | mobile | community | 1 | 36.0 | 3659 | 0 |
| logged-in-reader | mobile | gacha | 2 | 31.5 | 2877 | 0 |
| logged-in-reader | mobile | gamification | 1 | 32.0 | 2899 | 0 |
| logged-in-reader | mobile | home | 1 | 26.0 | 2703 | 0 |
| logged-in-reader | mobile | inventory | 1 | 30.0 | 2854 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 33.0 | 2845 | 0 |
| logged-in-reader | mobile | legal | 3 | 25.0 | 2691 | 0 |
| logged-in-reader | mobile | notifications | 1 | 31.0 | 2867 | 0 |
| logged-in-reader | mobile | profile | 3 | 29.3 | 2849 | 0 |
| logged-in-reader | mobile | reader | 1 | 28.0 | 2778 | 0 |
| logged-in-reader | mobile | readers | 7 | 28.0 | 2788 | 0 |
| logged-in-reader | mobile | reading | 5 | 28.6 | 2754 | 0 |
| logged-in-reader | mobile | wallet | 4 | 36.3 | 3059 | 0 |
| logged-out | desktop | auth | 5 | 24.0 | 2705 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 4062 | 0 |
| logged-out | desktop | legal | 3 | 25.0 | 2697 | 0 |
| logged-out | mobile | auth | 5 | 24.2 | 2684 | 0 |
| logged-out | mobile | home | 1 | 30.0 | 3327 | 0 |
| logged-out | mobile | legal | 3 | 25.0 | 2703 | 0 |

## Key Findings
1. Critical: 5 page(s) vượt 35 requests.
2. High: 142 page(s) vượt 25 requests.
3. High: 35 request vượt 800ms.
4. Medium: 183 request trong dải 400-800ms.
5. High: phát hiện 3 handshake redirect(s), cần kiểm tra vòng lặp auth/session.
6. Collection-focus: 8 image request(s) >800ms trên 38 image request(s).

## Top Slow Pages
| Scenario | Viewport | Feature | Route | Navigate (ms) | Request count | LCP (ms) | TBT (ms) | CLS |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 6259 | 29 | 508 | 0.0 | 0.0040 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 6171 | 29 | 628 | 0.0 | 0.0042 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 5560 | 29 | 468 | 0.0 | 0.0000 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 5490 | 29 | 432 | 0.0 | 0.0000 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 4118 | 35 | 2224 | 0.0 | 0.0041 |
| logged-out | desktop | auth-public | /vi | 4062 | 29 | 1532 | 367.0 | 0.0000 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 3792 | 55 | 1168 | 0.0 | 0.0000 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3779 | 55 | 1436 | 33.0 | 0.0000 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 3765 | 55 | 1424 | 37.0 | 0.0000 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 3706 | 32 | 1424 | 0.0 | 0.0760 |

## High Slow Requests (> 800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 1366 | 310 | https://www.tarotnow.xyz/vi/profile |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1340 | 596 | https://www.tarotnow.xyz/vi |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 1237 | 772 | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 1171 | 311 | https://www.tarotnow.xyz/vi/reading |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1072 | 572 | https://www.tarotnow.xyz/_next/static/chunks/0tl7_1-s~69.7.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 1009 | 515 | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1200&q=75 |
| logged-out | mobile | auth-public | /vi | GET | 200 | 978 | 555 | https://www.tarotnow.xyz/vi |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 855 | 598 | https://www.tarotnow.xyz/_next/static/chunks/172o3d9qvywlw.js |
| logged-in-admin | desktop | reading | /vi/reading | GET | 200 | 840 | 366 | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 838 | 335 | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 837 | 397 | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 835 | 316 | https://www.tarotnow.xyz/vi/profile |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 834 | 323 | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 832 | 628 | https://www.tarotnow.xyz/_next/static/chunks/04nria0ddfo_c.js |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 831 | 455 | https://www.tarotnow.xyz/vi/community |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-out | desktop | auth-public | /vi | GET | 200 | 800 | 555 | https://www.tarotnow.xyz/_next/static/chunks/0s_trpk6aw84g.js |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 800 | 320 | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 794 | 325 | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 793 | 346 | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 793 | 639 | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 791 | 334 | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | mobile | reader-chat | /vi/readers | GET | 200 | 791 | 401 | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 790 | 693 | https://www.tarotnow.xyz/_next/static/chunks/0qw8t1w~-.eb1.js |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 787 | 315 | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 786 | 314 | https://www.tarotnow.xyz/vi/gamification |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 786 | 315 | https://www.tarotnow.xyz/vi/inventory |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 785 | 310 | https://www.tarotnow.xyz/vi/inventory |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 785 | 305 | https://www.tarotnow.xyz/vi/wallet |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 785 | 306 | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 783 | 103 | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |

## Duplicate Request Candidates (Non-telemetry)
| Scenario | Viewport | Feature | Route | Count | Request Key |
| --- | --- | --- | --- | ---: | --- |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/04nria0ddfo_c.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0xovby2vf96zv.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0d2b1s5.nlpim.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0tl7_1-s~69.7.js |

## Collection Image Metrics
| Scenario | Viewport | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | 13 | 4 | 4 | 12 | 1 | 0 |
| logged-in-reader | desktop | 6 | 4 | 0 | 6 | 0 | 0 |
| logged-in-admin | mobile | 13 | 4 | 4 | 12 | 1 | 0 |
| logged-in-reader | mobile | 6 | 1 | 0 | 6 | 0 | 0 |

## Notes
- Duplicate `/cdn-cgi/rum` được xem là telemetry của Cloudflare, không coi là business over-fetch.
- Coverage dynamic route có thể bị `coverage-blocked` nếu environment không có dữ liệu phù hợp tại thời điểm chạy.