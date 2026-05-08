# TarotNow Benchmark Analysis

- Run time (UTC): 2026-05-08T19:08:23.915Z
- Base: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Matrix: Chromium desktop + mobile, logged-out + logged-in-admin + logged-in-reader

## Scenario Summary
| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 24.9 | 2825 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 29.6 | 2941 | 0 | 0 | 1 | 15 | 0 | yes |
| logged-in-reader | desktop | 33 | 29.6 | 2936 | 0 | 0 | 1 | 17 | 1 | yes |
| logged-out | mobile | 9 | 24.7 | 2774 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 30.7 | 2941 | 0 | 0 | 2 | 11 | 0 | yes |
| logged-in-reader | mobile | 33 | 30.4 | 2969 | 0 | 0 | 1 | 22 | 1 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.4 | 2887 | 0 |
| logged-in-admin | desktop | chat | 1 | 30.0 | 2885 | 0 |
| logged-in-admin | desktop | collection | 1 | 29.0 | 5953 | 0 |
| logged-in-admin | desktop | community | 1 | 36.0 | 3936 | 0 |
| logged-in-admin | desktop | gacha | 2 | 31.5 | 2865 | 0 |
| logged-in-admin | desktop | gamification | 1 | 31.0 | 3270 | 0 |
| logged-in-admin | desktop | home | 1 | 26.0 | 2680 | 0 |
| logged-in-admin | desktop | inventory | 1 | 33.0 | 2870 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 32.0 | 3246 | 0 |
| logged-in-admin | desktop | legal | 3 | 25.0 | 2750 | 0 |
| logged-in-admin | desktop | notifications | 1 | 28.0 | 2712 | 0 |
| logged-in-admin | desktop | profile | 3 | 29.0 | 2830 | 0 |
| logged-in-admin | desktop | reader | 1 | 28.0 | 2767 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.4 | 2748 | 0 |
| logged-in-admin | desktop | reading | 5 | 28.6 | 2747 | 0 |
| logged-in-admin | desktop | wallet | 4 | 35.0 | 2955 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.4 | 2784 | 0 |
| logged-in-admin | mobile | chat | 1 | 28.0 | 2780 | 0 |
| logged-in-admin | mobile | collection | 1 | 29.0 | 5623 | 0 |
| logged-in-admin | mobile | community | 1 | 30.0 | 3564 | 0 |
| logged-in-admin | mobile | gacha | 2 | 31.5 | 2865 | 0 |
| logged-in-admin | mobile | gamification | 1 | 29.0 | 2878 | 0 |
| logged-in-admin | mobile | home | 1 | 26.0 | 2772 | 0 |
| logged-in-admin | mobile | inventory | 1 | 32.0 | 2884 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 33.0 | 2922 | 0 |
| logged-in-admin | mobile | legal | 3 | 25.0 | 2684 | 0 |
| logged-in-admin | mobile | notifications | 1 | 28.0 | 2744 | 0 |
| logged-in-admin | mobile | profile | 3 | 29.0 | 2846 | 0 |
| logged-in-admin | mobile | reader | 1 | 28.0 | 2947 | 0 |
| logged-in-admin | mobile | readers | 7 | 28.3 | 2761 | 0 |
| logged-in-admin | mobile | reading | 5 | 29.6 | 2852 | 0 |
| logged-in-admin | mobile | wallet | 4 | 48.0 | 3400 | 0 |
| logged-in-reader | desktop | chat | 1 | 28.0 | 2752 | 0 |
| logged-in-reader | desktop | collection | 1 | 29.0 | 6231 | 0 |
| logged-in-reader | desktop | community | 1 | 30.0 | 3428 | 0 |
| logged-in-reader | desktop | gacha | 2 | 33.0 | 2849 | 0 |
| logged-in-reader | desktop | gamification | 1 | 30.0 | 2893 | 0 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2712 | 0 |
| logged-in-reader | desktop | inventory | 1 | 32.0 | 2837 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 32.0 | 2860 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.0 | 2657 | 0 |
| logged-in-reader | desktop | notifications | 1 | 29.0 | 2843 | 0 |
| logged-in-reader | desktop | profile | 3 | 29.7 | 2945 | 0 |
| logged-in-reader | desktop | reader | 1 | 28.0 | 2660 | 0 |
| logged-in-reader | desktop | readers | 7 | 28.0 | 2778 | 0 |
| logged-in-reader | desktop | reading | 5 | 28.6 | 2725 | 0 |
| logged-in-reader | desktop | wallet | 4 | 36.0 | 3023 | 0 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 2698 | 0 |
| logged-in-reader | mobile | collection | 1 | 29.0 | 5597 | 0 |
| logged-in-reader | mobile | community | 1 | 30.0 | 3580 | 0 |
| logged-in-reader | mobile | gacha | 2 | 33.0 | 2870 | 0 |
| logged-in-reader | mobile | gamification | 1 | 31.0 | 2909 | 0 |
| logged-in-reader | mobile | home | 1 | 35.0 | 2864 | 0 |
| logged-in-reader | mobile | inventory | 1 | 33.0 | 2845 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 32.0 | 2851 | 0 |
| logged-in-reader | mobile | legal | 3 | 25.0 | 2705 | 0 |
| logged-in-reader | mobile | notifications | 1 | 31.0 | 2884 | 0 |
| logged-in-reader | mobile | profile | 3 | 29.7 | 3064 | 0 |
| logged-in-reader | mobile | reader | 1 | 28.0 | 2750 | 0 |
| logged-in-reader | mobile | readers | 7 | 28.9 | 2774 | 0 |
| logged-in-reader | mobile | reading | 5 | 29.8 | 2854 | 0 |
| logged-in-reader | mobile | wallet | 4 | 36.0 | 3066 | 0 |
| logged-out | desktop | auth | 5 | 24.0 | 2702 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 3772 | 0 |
| logged-out | desktop | legal | 3 | 25.0 | 2716 | 0 |
| logged-out | mobile | auth | 5 | 24.2 | 2722 | 0 |
| logged-out | mobile | home | 1 | 26.0 | 3090 | 0 |
| logged-out | mobile | legal | 3 | 25.0 | 2756 | 0 |

## Key Findings
1. Critical: 6 page(s) vượt 35 requests.
2. High: 142 page(s) vượt 25 requests.
3. High: 30 request vượt 800ms.
4. Medium: 188 request trong dải 400-800ms.
5. High: phát hiện 5 handshake redirect(s), cần kiểm tra vòng lặp auth/session.
6. Collection-focus: 3 image request(s) >800ms trên 38 image request(s).

## Top Slow Pages
| Scenario | Viewport | Feature | Route | Navigate (ms) | Request count | LCP (ms) | TBT (ms) | CLS |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 6231 | 29 | 588 | 0.0 | 0.0040 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 5953 | 29 | 480 | 0.0 | 0.0042 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 5623 | 29 | 476 | 0.0 | 0.0000 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 5597 | 29 | 780 | 0.0 | 0.0000 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 4377 | 80 | 808 | 0.0 | 0.0055 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 3936 | 36 | 1880 | 0.0 | 0.0041 |
| logged-out | desktop | auth-public | /vi | 3772 | 29 | 1080 | 527.0 | 0.0000 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 3719 | 33 | 1440 | 0.0 | 0.0892 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 3713 | 56 | 1100 | 0.0 | 0.0000 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 3681 | 56 | 1348 | 28.0 | 0.0000 |

## High Slow Requests (> 800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 1381 | 327 | https://www.tarotnow.xyz/vi/profile |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 1113 | 317 | https://www.tarotnow.xyz/vi/profile |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1099 | 589 | https://www.tarotnow.xyz/_next/static/chunks/08nqfkhj3~_69.js |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 1009 | 519 | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-out | mobile | auth-public | /vi | GET | 200 | 941 | 551 | https://www.tarotnow.xyz/vi |
| logged-in-reader | desktop | reader-chat | /vi/readers | GET | 200 | 923 | 339 | https://www.tarotnow.xyz/vi/readers |
| logged-out | desktop | auth-public | /vi | GET | 200 | 907 | 345 | https://www.tarotnow.xyz/vi |
| logged-in-admin | desktop | auth-public | /vi/legal/tos | GET | 200 | 905 | 573 | https://www.tarotnow.xyz/vi/legal/tos |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 894 | 529 | https://www.tarotnow.xyz/vi/reading/history |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 866 | 385 | https://www.tarotnow.xyz/vi/profile |
| logged-out | desktop | auth-public | /vi | GET | 200 | 856 | 558 | https://www.tarotnow.xyz/_next/static/chunks/08f_jn-s1oto5.js |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | GET | 200 | 856 | 552 | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-admin | desktop | admin | /vi/admin/disputes | GET | 200 | 851 | 361 | https://www.tarotnow.xyz/vi/admin/disputes |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 836 | 335 | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 834 | 340 | https://www.tarotnow.xyz/vi/profile |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-reader | desktop | reading | /vi/reading | GET | 200 | 800 | 317 | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 800 | 314 | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | mobile | reading | /vi/reading | GET | 200 | 799 | 312 | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | desktop | reader-chat | /vi/readers | GET | 200 | 798 | 331 | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | desktop | reader-chat | /vi/chat | GET | 200 | 792 | 369 | https://www.tarotnow.xyz/vi/chat |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 792 | 333 | https://www.tarotnow.xyz/vi/gamification |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 791 | 311 | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 791 | 329 | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | desktop | reading | /vi/reading | GET | 200 | 785 | 315 | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 783 | 319 | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | GET | 200 | 782 | 329 | https://www.tarotnow.xyz/vi/notifications |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 782 | 299 | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 779 | 386 | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 776 | 538 | https://www.tarotnow.xyz/_next/static/chunks/08pjgbp2d2w~p.js |
| logged-in-admin | desktop | admin | /vi/admin/promotions | GET | 200 | 773 | 340 | https://www.tarotnow.xyz/vi/admin/promotions |

## Duplicate Request Candidates (Non-telemetry)
| Scenario | Viewport | Feature | Route | Count | Request Key |
| --- | --- | --- | --- | ---: | --- |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/0o0619o11z6de.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/04rddris79wjy.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/08nqfkhj3~_69.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/08f_jn-s1oto5.js |

## Collection Image Metrics
| Scenario | Viewport | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | 13 | 4 | 3 | 12 | 1 | 0 |
| logged-in-reader | desktop | 6 | 0 | 0 | 6 | 0 | 0 |
| logged-in-admin | mobile | 13 | 5 | 0 | 12 | 1 | 0 |
| logged-in-reader | mobile | 6 | 4 | 0 | 6 | 0 | 0 |

## Notes
- Duplicate `/cdn-cgi/rum` được xem là telemetry của Cloudflare, không coi là business over-fetch.
- Coverage dynamic route có thể bị `coverage-blocked` nếu environment không có dữ liệu phù hợp tại thời điểm chạy.