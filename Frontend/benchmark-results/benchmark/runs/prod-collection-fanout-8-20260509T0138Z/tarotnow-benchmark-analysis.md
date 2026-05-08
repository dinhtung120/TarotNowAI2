# TarotNow Benchmark Analysis

- Run time (UTC): 2026-05-08T18:46:27.352Z
- Base: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Matrix: Chromium desktop + mobile, logged-out + logged-in-admin + logged-in-reader

## Scenario Summary
| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 24.9 | 2845 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 29.7 | 3007 | 1 | 0 | 1 | 13 | 1 | yes |
| logged-in-reader | desktop | 33 | 29.4 | 2950 | 9 | 0 | 0 | 15 | 0 | yes |
| logged-out | mobile | 9 | 25.0 | 2799 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 29.7 | 2898 | 0 | 0 | 1 | 10 | 3 | yes |
| logged-in-reader | mobile | 33 | 29.9 | 2942 | 0 | 0 | 1 | 18 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.3 | 2915 | 0 |
| logged-in-admin | desktop | chat | 1 | 30.0 | 2899 | 0 |
| logged-in-admin | desktop | collection | 1 | 31.0 | 6677 | 0 |
| logged-in-admin | desktop | community | 1 | 36.0 | 4095 | 0 |
| logged-in-admin | desktop | gacha | 2 | 30.5 | 2919 | 0 |
| logged-in-admin | desktop | gamification | 1 | 31.0 | 3261 | 1 |
| logged-in-admin | desktop | home | 1 | 26.0 | 2731 | 0 |
| logged-in-admin | desktop | inventory | 1 | 30.0 | 2849 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 32.0 | 3250 | 0 |
| logged-in-admin | desktop | legal | 3 | 25.0 | 2742 | 0 |
| logged-in-admin | desktop | notifications | 1 | 30.0 | 2891 | 0 |
| logged-in-admin | desktop | profile | 3 | 29.0 | 2872 | 0 |
| logged-in-admin | desktop | reader | 1 | 28.0 | 2756 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.3 | 2806 | 0 |
| logged-in-admin | desktop | reading | 5 | 29.8 | 2850 | 0 |
| logged-in-admin | desktop | wallet | 4 | 35.5 | 3042 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.3 | 2807 | 0 |
| logged-in-admin | mobile | chat | 1 | 28.0 | 2802 | 0 |
| logged-in-admin | mobile | collection | 1 | 29.0 | 5455 | 0 |
| logged-in-admin | mobile | community | 1 | 36.0 | 3629 | 0 |
| logged-in-admin | mobile | gacha | 2 | 31.0 | 2872 | 0 |
| logged-in-admin | mobile | gamification | 1 | 29.0 | 2935 | 0 |
| logged-in-admin | mobile | home | 1 | 26.0 | 2756 | 0 |
| logged-in-admin | mobile | inventory | 1 | 30.0 | 2780 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 30.0 | 2872 | 0 |
| logged-in-admin | mobile | legal | 3 | 25.0 | 2691 | 0 |
| logged-in-admin | mobile | notifications | 1 | 28.0 | 2716 | 0 |
| logged-in-admin | mobile | profile | 3 | 29.0 | 2842 | 0 |
| logged-in-admin | mobile | reader | 1 | 28.0 | 2740 | 0 |
| logged-in-admin | mobile | readers | 7 | 28.3 | 2774 | 0 |
| logged-in-admin | mobile | reading | 5 | 29.8 | 2802 | 0 |
| logged-in-admin | mobile | wallet | 4 | 36.8 | 3025 | 0 |
| logged-in-reader | desktop | chat | 1 | 28.0 | 2748 | 0 |
| logged-in-reader | desktop | collection | 1 | 29.0 | 6132 | 0 |
| logged-in-reader | desktop | community | 1 | 37.0 | 3605 | 0 |
| logged-in-reader | desktop | gacha | 2 | 31.5 | 2896 | 0 |
| logged-in-reader | desktop | gamification | 1 | 33.0 | 3463 | 0 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2754 | 0 |
| logged-in-reader | desktop | inventory | 1 | 30.0 | 2839 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 30.0 | 2738 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.0 | 2684 | 0 |
| logged-in-reader | desktop | notifications | 1 | 28.0 | 2840 | 0 |
| logged-in-reader | desktop | profile | 3 | 29.0 | 2826 | 0 |
| logged-in-reader | desktop | reader | 1 | 28.0 | 2711 | 0 |
| logged-in-reader | desktop | readers | 7 | 28.3 | 2801 | 0 |
| logged-in-reader | desktop | reading | 5 | 33.0 | 2906 | 0 |
| logged-in-reader | desktop | wallet | 4 | 28.0 | 2765 | 9 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 2783 | 0 |
| logged-in-reader | mobile | collection | 1 | 31.0 | 5573 | 0 |
| logged-in-reader | mobile | community | 1 | 30.0 | 3533 | 0 |
| logged-in-reader | mobile | gacha | 2 | 32.5 | 2867 | 0 |
| logged-in-reader | mobile | gamification | 1 | 29.0 | 2842 | 0 |
| logged-in-reader | mobile | home | 1 | 26.0 | 2745 | 0 |
| logged-in-reader | mobile | inventory | 1 | 32.0 | 2855 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 30.0 | 2818 | 0 |
| logged-in-reader | mobile | legal | 3 | 25.0 | 2729 | 0 |
| logged-in-reader | mobile | notifications | 1 | 28.0 | 2752 | 0 |
| logged-in-reader | mobile | profile | 3 | 30.7 | 2872 | 0 |
| logged-in-reader | mobile | reader | 1 | 28.0 | 2721 | 0 |
| logged-in-reader | mobile | readers | 7 | 29.0 | 2808 | 0 |
| logged-in-reader | mobile | reading | 5 | 29.8 | 2822 | 0 |
| logged-in-reader | mobile | wallet | 4 | 35.5 | 3043 | 0 |
| logged-out | desktop | auth | 5 | 24.0 | 2731 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 3903 | 0 |
| logged-out | desktop | legal | 3 | 25.0 | 2683 | 0 |
| logged-out | mobile | auth | 5 | 24.0 | 2747 | 0 |
| logged-out | mobile | home | 1 | 30.0 | 3190 | 0 |
| logged-out | mobile | legal | 3 | 25.0 | 2754 | 0 |

## Key Findings
1. Critical: 7 page(s) vượt 35 requests.
2. High: 142 page(s) vượt 25 requests.
3. High: 27 request vượt 800ms.
4. Medium: 218 request trong dải 400-800ms.
5. High: phát hiện 3 handshake redirect(s), cần kiểm tra vòng lặp auth/session.
6. Collection-focus: 8 image request(s) >800ms trên 38 image request(s).

## Top Slow Pages
| Scenario | Viewport | Feature | Route | Navigate (ms) | Request count | LCP (ms) | TBT (ms) | CLS |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 6677 | 31 | 884 | 0.0 | 0.0042 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 6132 | 29 | 516 | 0.0 | 0.0040 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 5573 | 31 | 792 | 0.0 | 0.0000 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 5455 | 29 | 584 | 0.0 | 0.0000 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 4095 | 36 | 1884 | 0.0 | 0.0041 |
| logged-out | desktop | auth-public | /vi | 3903 | 29 | 1324 | 243.0 | 0.0000 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 3805 | 56 | 1424 | 9.0 | 0.0000 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 3669 | 56 | 1104 | 0.0 | 0.0000 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 3649 | 56 | 1140 | 0.0 | 0.0000 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 3629 | 36 | 1824 | 0.0 | 0.0051 |

## High Slow Requests (> 800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1124 | 597 | https://www.tarotnow.xyz/vi |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1032 | 547 | https://www.tarotnow.xyz/_next/static/chunks/102-.gonu2j.f.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 1024 | 519 | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1200&q=75 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 987 | 509 | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 983 | 334 | https://www.tarotnow.xyz/vi/gamification |
| logged-out | mobile | auth-public | /vi | GET | 200 | 960 | 614 | https://www.tarotnow.xyz/vi |
| logged-out | mobile | auth-public | /vi/login | GET | 200 | 838 | 554 | https://www.tarotnow.xyz/vi/login |
| logged-in-admin | desktop | reader-chat | /vi/readers | GET | 200 | 837 | 440 | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 837 | 369 | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 832 | 351 | https://www.tarotnow.xyz/vi/wallet |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 830 | 313 | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | GET | 200 | 828 | 422 | https://www.tarotnow.xyz/vi/admin/reader-requests |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | GET | 200 | 827 | 342 | https://www.tarotnow.xyz/vi/notifications |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 827 | 349 | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 823 | 354 | https://www.tarotnow.xyz/vi/profile |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 798 | 307 | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 797 | 312 | https://www.tarotnow.xyz/vi/profile |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 796 | 324 | https://www.tarotnow.xyz/vi/inventory |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 796 | 326 | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 794 | 553 | https://www.tarotnow.xyz/_next/static/chunks/0ijiw4-281e4s.js |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 794 | 312 | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 794 | 316 | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | mobile | reader-chat | /vi/readers | GET | 200 | 793 | 326 | https://www.tarotnow.xyz/vi/readers |
| logged-in-reader | mobile | reading | /vi/reading | GET | 200 | 791 | 328 | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 786 | 300 | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-in-admin | desktop | reader-chat | /vi/chat | GET | 200 | 785 | 377 | https://www.tarotnow.xyz/vi/chat |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 785 | 365 | https://www.tarotnow.xyz/vi/community |
| logged-in-reader | mobile | reader-chat | /vi/readers | GET | 200 | 783 | 312 | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | desktop | reading | /vi/reading | GET | 200 | 782 | 320 | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | desktop | reading | /vi/reading/history | GET | 200 | 782 | 389 | https://www.tarotnow.xyz/vi/reading/history |

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
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/07or0in3dm_w_.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/102-.gonu2j.f.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/020ipuq9bw_tk.js |

## Collection Image Metrics
| Scenario | Viewport | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | 13 | 3 | 5 | 12 | 1 | 0 |
| logged-in-reader | desktop | 6 | 3 | 0 | 6 | 0 | 0 |
| logged-in-admin | mobile | 13 | 4 | 1 | 12 | 1 | 0 |
| logged-in-reader | mobile | 6 | 1 | 2 | 6 | 0 | 0 |

## Notes
- Duplicate `/cdn-cgi/rum` được xem là telemetry của Cloudflare, không coi là business over-fetch.
- Coverage dynamic route có thể bị `coverage-blocked` nếu environment không có dữ liệu phù hợp tại thời điểm chạy.