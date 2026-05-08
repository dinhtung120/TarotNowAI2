# TarotNow Navigation Benchmark Report

- Generated at (UTC): 2026-05-08T20:21:39.520Z
- Base URL: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Thresholds: >35 requests = Critical, >25 = High, request >800ms = High, >400ms = Medium
- Critical pages (request count): 2
- High pages (request count): 143
- High slow requests: 131
- Medium slow requests: 502

## Scenario Summary
| Scenario | Viewport | Pages Benchmarked | Avg Navigation (ms) | Total Requests | Pending Requests | Document Reloads | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 2782 | 221 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 2924 | 1234 | 0 | 0 | 15 | 0 | yes |
| logged-in-reader | desktop | 33 | 3132 | 964 | 1 | 0 | 14 | 0 | yes |
| logged-out | mobile | 9 | 2941 | 226 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 3139 | 1259 | 2 | 0 | 20 | 0 | yes |
| logged-in-reader | mobile | 33 | 3401 | 966 | 1 | 0 | 19 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.0 | 2759 | 0 |
| logged-in-admin | desktop | chat | 1 | 28.0 | 2757 | 0 |
| logged-in-admin | desktop | collection | 1 | 29.0 | 6727 | 0 |
| logged-in-admin | desktop | community | 1 | 30.0 | 3585 | 0 |
| logged-in-admin | desktop | gacha | 2 | 31.0 | 2935 | 0 |
| logged-in-admin | desktop | gamification | 1 | 30.0 | 2889 | 0 |
| logged-in-admin | desktop | home | 1 | 26.0 | 2709 | 0 |
| logged-in-admin | desktop | inventory | 1 | 32.0 | 2913 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 32.0 | 2917 | 0 |
| logged-in-admin | desktop | legal | 3 | 25.0 | 2732 | 0 |
| logged-in-admin | desktop | notifications | 1 | 28.0 | 2741 | 0 |
| logged-in-admin | desktop | profile | 3 | 29.0 | 2821 | 0 |
| logged-in-admin | desktop | reader | 1 | 28.0 | 2884 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.1 | 2819 | 0 |
| logged-in-admin | desktop | reading | 5 | 29.2 | 2873 | 0 |
| logged-in-admin | desktop | wallet | 4 | 28.5 | 2855 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.5 | 2895 | 0 |
| logged-in-admin | mobile | chat | 1 | 28.0 | 3023 | 0 |
| logged-in-admin | mobile | collection | 1 | 29.0 | 6208 | 0 |
| logged-in-admin | mobile | community | 1 | 35.0 | 3750 | 0 |
| logged-in-admin | mobile | gacha | 2 | 31.5 | 2910 | 0 |
| logged-in-admin | mobile | gamification | 1 | 29.0 | 2905 | 0 |
| logged-in-admin | mobile | home | 1 | 26.0 | 2781 | 0 |
| logged-in-admin | mobile | inventory | 1 | 31.0 | 2907 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 31.0 | 2912 | 0 |
| logged-in-admin | mobile | legal | 3 | 25.7 | 3403 | 0 |
| logged-in-admin | mobile | notifications | 1 | 28.0 | 3754 | 0 |
| logged-in-admin | mobile | profile | 3 | 28.7 | 2970 | 0 |
| logged-in-admin | mobile | reader | 1 | 29.0 | 3569 | 0 |
| logged-in-admin | mobile | readers | 7 | 28.4 | 3034 | 0 |
| logged-in-admin | mobile | reading | 5 | 31.6 | 3112 | 0 |
| logged-in-admin | mobile | wallet | 4 | 28.8 | 3121 | 2 |
| logged-in-reader | desktop | chat | 1 | 28.0 | 2835 | 0 |
| logged-in-reader | desktop | collection | 1 | 29.0 | 6663 | 0 |
| logged-in-reader | desktop | community | 1 | 38.0 | 4460 | 1 |
| logged-in-reader | desktop | gacha | 2 | 33.0 | 3309 | 0 |
| logged-in-reader | desktop | gamification | 1 | 30.0 | 2965 | 0 |
| logged-in-reader | desktop | home | 1 | 29.0 | 3176 | 0 |
| logged-in-reader | desktop | inventory | 1 | 30.0 | 2883 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 31.0 | 3211 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.0 | 2787 | 0 |
| logged-in-reader | desktop | notifications | 1 | 29.0 | 3291 | 0 |
| logged-in-reader | desktop | profile | 3 | 28.7 | 2915 | 0 |
| logged-in-reader | desktop | reader | 1 | 28.0 | 2798 | 0 |
| logged-in-reader | desktop | readers | 7 | 28.0 | 2877 | 0 |
| logged-in-reader | desktop | reading | 5 | 31.0 | 2958 | 0 |
| logged-in-reader | desktop | wallet | 4 | 28.5 | 3109 | 0 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 2925 | 0 |
| logged-in-reader | mobile | collection | 1 | 29.0 | 6369 | 0 |
| logged-in-reader | mobile | community | 1 | 35.0 | 3722 | 0 |
| logged-in-reader | mobile | gacha | 2 | 31.5 | 2953 | 0 |
| logged-in-reader | mobile | gamification | 1 | 29.0 | 2962 | 0 |
| logged-in-reader | mobile | home | 1 | 34.0 | 4550 | 0 |
| logged-in-reader | mobile | inventory | 1 | 31.0 | 2918 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 30.0 | 3047 | 0 |
| logged-in-reader | mobile | legal | 3 | 25.0 | 3576 | 0 |
| logged-in-reader | mobile | notifications | 1 | 29.0 | 4261 | 0 |
| logged-in-reader | mobile | profile | 3 | 30.3 | 3419 | 0 |
| logged-in-reader | mobile | reader | 1 | 28.0 | 3529 | 0 |
| logged-in-reader | mobile | readers | 7 | 28.4 | 2922 | 0 |
| logged-in-reader | mobile | reading | 5 | 30.2 | 3315 | 0 |
| logged-in-reader | mobile | wallet | 4 | 28.5 | 3507 | 1 |
| logged-out | desktop | auth | 5 | 24.0 | 2758 | 0 |
| logged-out | desktop | home | 1 | 26.0 | 3071 | 0 |
| logged-out | desktop | legal | 3 | 25.0 | 2725 | 0 |
| logged-out | mobile | auth | 5 | 24.2 | 2721 | 0 |
| logged-out | mobile | home | 1 | 30.0 | 3395 | 0 |
| logged-out | mobile | legal | 3 | 25.0 | 3156 | 0 |

## Per-Page Metrics
| Scenario | Viewport | Feature | Route | Requests | Interaction Requests | Severity | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Collection Img Requests | Collection Img 400-800ms | Collection Img >800ms | Collection Img First Load | Collection Img Reopen | Collection Img Cache Hits | Navigate (ms) | DOMContentLoaded (ms) | Load (ms) | FCP (ms) | LCP (ms) | CLS | TBT (ms) | Transfer Bytes |
| --- | --- | --- | --- | ---: | ---: | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-out | desktop | auth-public | /vi | 26 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3071 | 1035 | 1036 | 860 | 1432 | 0.0000 | 171.0 | 530456 |
| logged-out | desktop | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2688 | 673 | 673 | 576 | 576 | 0.0000 | 0.0 | 511740 |
| logged-out | desktop | auth-public | /vi/register | 24 | 2 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2717 | 702 | 702 | 532 | 532 | 0.0000 | 0.0 | 512260 |
| logged-out | desktop | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2860 | 840 | 840 | 688 | 688 | 0.0000 | 0.0 | 511550 |
| logged-out | desktop | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2776 | 754 | 754 | 632 | 632 | 0.0000 | 0.0 | 511405 |
| logged-out | desktop | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2750 | 736 | 736 | 540 | 540 | 0.0000 | 0.0 | 511542 |
| logged-out | desktop | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2797 | 786 | 786 | 736 | 736 | 0.0000 | 0.0 | 525307 |
| logged-out | desktop | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2689 | 671 | 671 | 476 | 792 | 0.0000 | 0.0 | 525252 |
| logged-out | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2688 | 674 | 675 | 464 | 776 | 0.0000 | 0.0 | 525459 |
| logged-in-admin | desktop | auth-public | /vi | 26 | 12 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2709 | 691 | 691 | 640 | 1284 | 0.0035 | 361.0 | 536980 |
| logged-in-admin | desktop | reading | /vi/reading | 29 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2933 | 878 | 890 | 492 | 1028 | 0.0041 | 0.0 | 642459 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 32 | 8 | high | 0 | 0 | 2 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2913 | 902 | 902 | 540 | 1196 | 0.0041 | 0.0 | 645743 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 32 | 9 | high | 0 | 0 | 2 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2896 | 885 | 885 | 520 | 1188 | 0.0041 | 0.0 | 726255 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 30 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2974 | 931 | 931 | 580 | 1304 | 0.0041 | 0.0 | 725471 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 29 | 27 | high | 0 | 0 | 0 | 0 | 13 | 3 | 5 | 12 | 1 | 0 | 6727 | 675 | 690 | 504 | 504 | 0.0042 | 0.0 | 642688 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2982 | 840 | 972 | 520 | 1208 | 0.0489 | 0.0 | 635247 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2766 | 738 | 751 | 524 | 968 | 0.0041 | 0.0 | 631242 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | 39 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2714 | 833 | 983 | 508 | 1228 | 0.0489 | 0.0 | 630225 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 29 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2950 | 928 | 929 | 532 | 1216 | 0.0041 | 0.0 | 634188 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2757 | 728 | 746 | 544 | 936 | 0.0041 | 0.0 | 630864 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 32 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2917 | 900 | 900 | 472 | 1168 | 0.0041 | 0.0 | 652138 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 30 | 9 | high | 0 | 0 | 0 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 3585 | 836 | 837 | 652 | 1896 | 0.0041 | 0.0 | 642490 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 30 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2889 | 856 | 856 | 532 | 996 | 0.0279 | 0.0 | 643293 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2927 | 905 | 905 | 536 | 1236 | 0.0041 | 0.0 | 634256 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2805 | 768 | 785 | 556 | 988 | 0.0041 | 0.0 | 631079 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2941 | 872 | 894 | 592 | 1224 | 0.0041 | 0.0 | 632546 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 38 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2745 | 835 | 835 | 620 | 1204 | 0.0041 | 0.0 | 630324 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2741 | 716 | 724 | 604 | 1040 | 0.0041 | 0.0 | 631565 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2884 | 845 | 855 | 540 | 932 | 0.0041 | 0.0 | 631907 |
| logged-in-admin | desktop | reading | /vi/reading/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2784 | 752 | 774 | 532 | 1180 | 0.0041 | 0.0 | 632247 |
| logged-in-admin | desktop | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2777 | 750 | 750 | 648 | 964 | 0.0020 | 0.0 | 525549 |
| logged-in-admin | desktop | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2715 | 702 | 702 | 552 | 888 | 0.0020 | 0.0 | 525582 |
| logged-in-admin | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2703 | 690 | 690 | 516 | 856 | 0.0020 | 0.0 | 525695 |
| logged-in-admin | desktop | admin | /vi/admin | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2758 | 717 | 739 | 560 | 900 | 0.0000 | 0.0 | 647211 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2774 | 740 | 755 | 640 | 1096 | 0.0000 | 0.0 | 647172 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2762 | 725 | 740 | 560 | 940 | 0.0000 | 0.0 | 645318 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 30 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2731 | 690 | 707 | 496 | 856 | 0.0022 | 0.0 | 664466 |
| logged-in-admin | desktop | admin | /vi/admin/promotions | 28 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2752 | 714 | 733 | 568 | 928 | 0.0000 | 0.0 | 644279 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2718 | 684 | 701 | 528 | 876 | 0.0000 | 0.0 | 645950 |
| logged-in-admin | desktop | admin | /vi/admin/readings | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2796 | 767 | 774 | 580 | 996 | 0.0000 | 0.0 | 648357 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | 28 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2786 | 753 | 766 | 552 | 1104 | 0.0000 | 0.0 | 655182 |
| logged-in-admin | desktop | admin | /vi/admin/users | 30 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2752 | 729 | 729 | 640 | 1048 | 0.0000 | 0.0 | 649394 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2757 | 709 | 727 | 536 | 916 | 0.0000 | 0.0 | 645526 |
| logged-in-admin | desktop | reading | /vi/reading/session/d622091d-18e5-46d0-83d9-750a4e40ed88 | 33 | 5 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3082 | 829 | 979 | 592 | 1040 | 0.0054 | 0.0 | 713723 |
| logged-in-admin | desktop | reading | /vi/reading/session/d25092e9-5583-4962-aea8-bb2f15a20746 | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2735 | 693 | 706 | 548 | 928 | 0.0041 | 0.0 | 631586 |
| logged-in-admin | desktop | reading | /vi/reading/session/37a37db8-b409-43b3-823d-74327215621e | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2830 | 779 | 797 | 680 | 1100 | 0.0041 | 0.0 | 631786 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2742 | 711 | 725 | 544 | 956 | 0.0041 | 0.0 | 630467 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2771 | 703 | 723 | 532 | 896 | 0.0041 | 0.0 | 630645 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2964 | 777 | 926 | 992 | 992 | 0.0041 | 0.0 | 630366 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2715 | 677 | 695 | 528 | 1076 | 0.0041 | 0.0 | 632270 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2860 | 797 | 829 | 552 | 1160 | 0.0041 | 0.0 | 632376 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2731 | 687 | 709 | 556 | 1136 | 0.0041 | 0.0 | 632451 |
| logged-in-reader | desktop | auth-public | /vi | 29 | 9 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3176 | 1129 | 1164 | 1184 | 1184 | 0.0033 | 349.0 | 607426 |
| logged-in-reader | desktop | reading | /vi/reading | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2990 | 933 | 938 | 564 | 1068 | 0.0039 | 0.0 | 641966 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 30 | 9 | high | 0 | 0 | 0 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2883 | 848 | 848 | 548 | 1168 | 0.0039 | 0.0 | 644083 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 36 | 5 | critical | 0 | 0 | 1 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 3733 | 843 | 1721 | 544 | 1032 | 0.0039 | 0.0 | 733323 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 30 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2884 | 848 | 848 | 544 | 1292 | 0.0039 | 0.0 | 723735 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 29 | 24 | high | 0 | 0 | 0 | 0 | 6 | 4 | 0 | 6 | 0 | 0 | 6663 | 817 | 912 | 532 | 944 | 0.0040 | 3.0 | 641814 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 30 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3071 | 855 | 1025 | 580 | 1236 | 0.0687 | 0.0 | 634558 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2821 | 753 | 790 | 536 | 980 | 0.0039 | 0.0 | 631064 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2854 | 818 | 836 | 620 | 1472 | 0.0039 | 0.0 | 631945 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2909 | 866 | 866 | 556 | 1204 | 0.0039 | 0.0 | 633631 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2835 | 795 | 813 | 600 | 1024 | 0.0039 | 0.0 | 631358 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 31 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3211 | 832 | 1195 | 564 | 1176 | 0.0039 | 0.0 | 651775 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 38 | 2 | critical | 0 | 0 | 2 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 4460 | 777 | 1698 | 584 | 1768 | 0.0039 | 0.0 | 787823 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 30 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2965 | 889 | 890 | 552 | 1052 | 0.0277 | 0.0 | 643363 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 29 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2928 | 879 | 879 | 588 | 1324 | 0.0039 | 0.0 | 634837 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2827 | 709 | 725 | 544 | 932 | 0.0039 | 0.0 | 631160 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3227 | 933 | 946 | 940 | 940 | 0.0039 | 0.0 | 631429 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3453 | 1187 | 1187 | 1008 | 1280 | 0.0095 | 0.0 | 633280 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3291 | 1252 | 1252 | 584 | 1056 | 0.0044 | 0.0 | 632683 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2798 | 771 | 783 | 568 | 964 | 0.0039 | 0.0 | 632134 |
| logged-in-reader | desktop | reading | /vi/reading/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2855 | 813 | 840 | 660 | 1332 | 0.0039 | 0.0 | 632596 |
| logged-in-reader | desktop | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2785 | 758 | 758 | 576 | 904 | 0.0019 | 0.0 | 525728 |
| logged-in-reader | desktop | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2784 | 765 | 765 | 800 | 800 | 0.0019 | 0.0 | 525572 |
| logged-in-reader | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2793 | 777 | 777 | 656 | 988 | 0.0019 | 0.0 | 525801 |
| logged-in-reader | desktop | reading | /vi/reading/session/f895dbb3-9491-4e19-abd1-ba807082827f | 35 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3077 | 806 | 1049 | 604 | 1028 | 0.0039 | 0.0 | 725929 |
| logged-in-reader | desktop | reading | /vi/reading/session/8234c561-c559-4b9a-9661-e81e2b31cd67 | 35 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3058 | 857 | 1027 | 552 | 964 | 0.0039 | 0.0 | 725974 |
| logged-in-reader | desktop | reading | /vi/reading/session/fcf1ef30-e19e-497e-b9c8-5305083365fe | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2809 | 767 | 794 | 604 | 992 | 0.0040 | 0.0 | 631687 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2872 | 754 | 821 | 540 | 912 | 0.0039 | 0.0 | 630929 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2779 | 738 | 758 | 612 | 980 | 0.0039 | 0.0 | 630668 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2780 | 711 | 730 | 572 | 940 | 0.0039 | 0.0 | 630901 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3008 | 925 | 936 | 604 | 1328 | 0.0039 | 0.0 | 632419 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2840 | 799 | 816 | 684 | 1392 | 0.0039 | 0.0 | 632637 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2948 | 841 | 916 | 548 | 1196 | 0.0039 | 0.0 | 632846 |
| logged-out | mobile | auth-public | /vi | 30 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3395 | 1365 | 1365 | 1016 | 1016 | 0.0000 | 0.0 | 602108 |
| logged-out | mobile | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2707 | 686 | 687 | 448 | 448 | 0.0000 | 0.0 | 511667 |
| logged-out | mobile | auth-public | /vi/register | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2743 | 723 | 723 | 480 | 480 | 0.0000 | 0.0 | 512844 |
| logged-out | mobile | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2691 | 671 | 671 | 468 | 468 | 0.0000 | 0.0 | 511308 |
| logged-out | mobile | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2719 | 688 | 688 | 456 | 456 | 0.0000 | 0.0 | 511368 |
| logged-out | mobile | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2744 | 722 | 722 | 612 | 612 | 0.0000 | 0.0 | 511483 |
| logged-out | mobile | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2832 | 734 | 734 | 500 | 824 | 0.0000 | 0.0 | 525156 |
| logged-out | mobile | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3387 | 971 | 1102 | 984 | 984 | 0.0000 | 0.0 | 525251 |
| logged-out | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3249 | 922 | 980 | 936 | 936 | 0.0000 | 0.0 | 525512 |
| logged-in-admin | mobile | auth-public | /vi | 26 | 12 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2781 | 751 | 751 | 504 | 868 | 0.0032 | 0.0 | 537021 |
| logged-in-admin | mobile | reading | /vi/reading | 30 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2903 | 855 | 890 | 488 | 868 | 0.0000 | 0.0 | 642820 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 31 | 8 | high | 0 | 0 | 1 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2907 | 890 | 890 | 492 | 1116 | 0.0000 | 0.0 | 644583 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 32 | 9 | high | 0 | 0 | 2 | 0 | 5 | 1 | 0 | 5 | 0 | 0 | 2907 | 891 | 891 | 492 | 1148 | 0.0000 | 0.0 | 726220 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 31 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2913 | 884 | 885 | 460 | 900 | 0.0000 | 0.0 | 726625 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 29 | 26 | high | 0 | 0 | 0 | 0 | 13 | 4 | 5 | 12 | 1 | 0 | 6208 | 815 | 862 | 500 | 832 | 0.0000 | 40.0 | 642505 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3003 | 883 | 989 | 452 | 1084 | 0.0689 | 0.0 | 634425 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2995 | 875 | 888 | 528 | 888 | 0.0000 | 0.0 | 631188 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | 37 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2911 | 1038 | 1038 | 784 | 1124 | 0.0760 | 0.0 | 630310 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2963 | 860 | 911 | 580 | 920 | 0.0000 | 0.0 | 633391 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3023 | 751 | 924 | 604 | 968 | 0.0000 | 0.0 | 631017 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 31 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2912 | 873 | 874 | 492 | 1132 | 0.0000 | 0.0 | 651162 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 35 | 3 | high | 0 | 0 | 1 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 3750 | 823 | 985 | 476 | 1668 | 0.0051 | 0.0 | 776416 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 29 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2905 | 867 | 875 | 496 | 876 | 0.0000 | 0.0 | 641959 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2951 | 921 | 921 | 468 | 1212 | 0.0000 | 0.0 | 634207 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2883 | 826 | 857 | 500 | 880 | 0.0000 | 0.0 | 631768 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2772 | 722 | 738 | 464 | 828 | 0.0000 | 0.0 | 631797 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 29 | 32 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3877 | 2099 | 2099 | 1176 | 1536 | 0.0071 | 0.0 | 630622 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3754 | 942 | 951 | 544 | 884 | 0.0003 | 0.0 | 631585 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | 29 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3569 | 1070 | 1089 | 536 | 884 | 0.0000 | 0.0 | 632508 |
| logged-in-admin | mobile | reading | /vi/reading/history | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3673 | 1221 | 1240 | 820 | 820 | 0.0071 | 0.0 | 632445 |
| logged-in-admin | mobile | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3751 | 1223 | 1223 | 1024 | 1340 | 0.0032 | 0.0 | 525629 |
| logged-in-admin | mobile | auth-public | /vi/legal/privacy | 27 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3597 | 1061 | 1061 | 644 | 948 | 0.0032 | 0.0 | 527436 |
| logged-in-admin | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2860 | 755 | 766 | 572 | 904 | 0.0032 | 0.0 | 525630 |
| logged-in-admin | mobile | admin | /vi/admin | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2929 | 663 | 892 | 552 | 876 | 0.0000 | 0.0 | 646998 |
| logged-in-admin | mobile | admin | /vi/admin/deposits | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2800 | 757 | 774 | 528 | 904 | 0.0000 | 0.0 | 647067 |
| logged-in-admin | mobile | admin | /vi/admin/disputes | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2815 | 765 | 785 | 480 | 796 | 0.0000 | 0.0 | 645427 |
| logged-in-admin | mobile | admin | /vi/admin/gamification | 32 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2957 | 811 | 924 | 468 | 808 | 0.0000 | 0.0 | 697780 |
| logged-in-admin | mobile | admin | /vi/admin/promotions | 28 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2896 | 790 | 791 | 536 | 872 | 0.0000 | 0.0 | 644042 |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2870 | 836 | 852 | 540 | 892 | 0.0000 | 0.0 | 645971 |
| logged-in-admin | mobile | admin | /vi/admin/readings | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2915 | 857 | 879 | 520 | 880 | 0.0000 | 0.0 | 648275 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | 30 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3017 | 865 | 966 | 536 | 936 | 0.0000 | 0.0 | 688894 |
| logged-in-admin | mobile | admin | /vi/admin/users | 31 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2931 | 794 | 877 | 468 | 1104 | 0.0000 | 0.0 | 651005 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2824 | 770 | 796 | 496 | 820 | 0.0000 | 0.0 | 645586 |
| logged-in-admin | mobile | reading | /vi/reading/session/68086b49-38df-4253-bb3b-51fbb3f82805 | 34 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2984 | 820 | 963 | 484 | 828 | 0.0000 | 0.0 | 693687 |
| logged-in-admin | mobile | reading | /vi/reading/session/01efc087-0c26-4c10-ab45-5f2c53406c64 | 33 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3017 | 852 | 995 | 484 | 884 | 0.0001 | 0.0 | 692910 |
| logged-in-admin | mobile | reading | /vi/reading/session/9caa0709-34bd-4cfd-a434-14056335f673 | 33 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2983 | 765 | 964 | 504 | 840 | 0.0000 | 0.0 | 692635 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2858 | 765 | 800 | 548 | 884 | 0.0000 | 0.0 | 630602 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2931 | 875 | 903 | 540 | 916 | 0.0000 | 0.0 | 631370 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2831 | 760 | 780 | 516 | 860 | 0.0000 | 0.0 | 630724 |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2845 | 768 | 822 | 488 | 832 | 0.0000 | 0.0 | 632240 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2998 | 878 | 883 | 504 | 956 | 0.0000 | 0.0 | 633065 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3813 | 1100 | 1120 | 500 | 864 | 0.0000 | 0.0 | 633263 |
| logged-in-reader | mobile | auth-public | /vi | 34 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4550 | 1466 | 2029 | 1528 | 1528 | 0.0032 | 10.0 | 610881 |
| logged-in-reader | mobile | reading | /vi/reading | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3160 | 1039 | 1057 | 520 | 900 | 0.0000 | 0.0 | 642114 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 31 | 8 | high | 0 | 0 | 1 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2918 | 874 | 874 | 480 | 1120 | 0.0000 | 0.0 | 644871 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 32 | 9 | high | 0 | 0 | 2 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2951 | 924 | 924 | 476 | 1132 | 0.0000 | 0.0 | 726189 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 31 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2954 | 861 | 881 | 484 | 1144 | 0.0000 | 0.0 | 725009 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 29 | 24 | high | 0 | 0 | 0 | 0 | 6 | 4 | 0 | 6 | 0 | 0 | 6369 | 912 | 921 | 584 | 912 | 0.0000 | 102.0 | 641792 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 33 | 4 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4474 | 2193 | 2456 | 484 | 2376 | 0.0892 | 0.0 | 638472 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 29 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2896 | 836 | 851 | 500 | 844 | 0.0000 | 0.0 | 632270 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2887 | 824 | 852 | 460 | 836 | 0.0000 | 0.0 | 632578 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 29 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2943 | 884 | 885 | 496 | 1132 | 0.0000 | 0.0 | 634164 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2925 | 775 | 831 | 484 | 840 | 0.0000 | 0.0 | 631398 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3047 | 963 | 963 | 484 | 1104 | 0.0000 | 0.0 | 650727 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 35 | 3 | high | 0 | 0 | 1 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 3722 | 842 | 917 | 480 | 1608 | 0.0051 | 0.0 | 776731 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 29 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2962 | 880 | 939 | 524 | 880 | 0.0000 | 0.0 | 642217 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2996 | 921 | 938 | 480 | 1172 | 0.0000 | 0.0 | 634232 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3137 | 844 | 911 | 552 | 912 | 0.0000 | 0.0 | 631308 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3194 | 862 | 869 | 744 | 744 | 0.0000 | 0.0 | 631328 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 29 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4701 | 1645 | 1654 | 980 | 1352 | 0.0401 | 0.0 | 633177 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4261 | 1498 | 1498 | 996 | 996 | 0.0000 | 0.0 | 632396 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3529 | 1098 | 1217 | 744 | 744 | 0.0071 | 0.0 | 631917 |
| logged-in-reader | mobile | reading | /vi/reading/history | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3940 | 1190 | 1214 | 1120 | 1120 | 0.0000 | 0.0 | 632505 |
| logged-in-reader | mobile | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3310 | 832 | 994 | 676 | 676 | 0.0032 | 0.0 | 525544 |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4054 | 1211 | 1211 | 1112 | 1112 | 0.0032 | 0.0 | 525586 |
| logged-in-reader | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3364 | 833 | 912 | 868 | 868 | 0.0032 | 0.0 | 525729 |
| logged-in-reader | mobile | reading | /vi/reading/session/e6e3bd1d-8c70-4a6d-afe6-f415b535ce9f | 28 | 8 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3380 | 794 | 801 | 532 | 3196 | 0.0001 | 0.0 | 631622 |
| logged-in-reader | mobile | reading | /vi/reading/session/4b40f3c2-2175-4d5a-91d7-23b281a8512d | 33 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3031 | 793 | 963 | 456 | 804 | 0.0000 | 0.0 | 692830 |
| logged-in-reader | mobile | reading | /vi/reading/session/e8ccde33-a90e-49f1-8b68-b0ec150b88c9 | 33 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3063 | 862 | 1010 | 492 | 832 | 0.0000 | 0.0 | 692934 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2866 | 819 | 829 | 492 | 816 | 0.0000 | 0.0 | 630661 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2922 | 859 | 894 | 516 | 864 | 0.0000 | 0.0 | 631477 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2938 | 858 | 868 | 476 | 828 | 0.0000 | 0.0 | 630750 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2919 | 813 | 863 | 480 | 864 | 0.0000 | 0.0 | 632452 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2879 | 801 | 811 | 468 | 816 | 0.0000 | 0.0 | 632688 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2989 | 910 | 912 | 508 | 892 | 0.0000 | 0.0 | 633326 |

## Collection Image Focus
| Scenario | Viewport | Route | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | /vi/collection | 13 | 3 | 5 | 12 | 1 | 0 |
| logged-in-reader | desktop | /vi/collection | 6 | 4 | 0 | 6 | 0 | 0 |
| logged-in-admin | mobile | /vi/collection | 13 | 4 | 5 | 12 | 1 | 0 |
| logged-in-reader | mobile | /vi/collection | 6 | 4 | 0 | 6 | 0 | 0 |

## Suspicious Pages (>25 requests)
| Scenario | Viewport | Feature | Route | Request Count | Severity | API | Static | Third-party |
| --- | --- | --- | --- | ---: | --- | ---: | ---: | ---: |
| logged-out | desktop | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-admin | desktop | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-admin | desktop | reading | /vi/reading | 29 | high | 1 | 26 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 32 | high | 2 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 32 | high | 2 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 31 | high | 2 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 29 | high | 1 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 32 | high | 2 | 28 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 30 | high | 1 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 29 | high | 1 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 29 | high | 1 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/promotions | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/readings | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/users | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/d622091d-18e5-46d0-83d9-750a4e40ed88 | 33 | high | 2 | 29 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/d25092e9-5583-4962-aea8-bb2f15a20746 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/37a37db8-b409-43b3-823d-74327215621e | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | auth-public | /vi | 29 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | reading | /vi/reading | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 36 | critical | 1 | 33 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 30 | high | 1 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 31 | high | 1 | 28 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 38 | critical | 3 | 33 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 30 | high | 1 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/f895dbb3-9491-4e19-abd1-ba807082827f | 35 | high | 3 | 30 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/8234c561-c559-4b9a-9661-e81e2b31cd67 | 35 | high | 3 | 30 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/fcf1ef30-e19e-497e-b9c8-5305083365fe | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-out | mobile | auth-public | /vi | 30 | high | 1 | 27 | 0 |
| logged-in-admin | mobile | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-admin | mobile | reading | /vi/reading | 30 | high | 2 | 26 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 31 | high | 1 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 32 | high | 2 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 31 | high | 1 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 30 | high | 1 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 31 | high | 1 | 28 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 35 | high | 1 | 32 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 29 | high | 1 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 29 | high | 1 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 29 | high | 1 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | 29 | high | 1 | 26 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | auth-public | /vi/legal/privacy | 27 | high | 1 | 23 | 0 |
| logged-in-admin | mobile | admin | /vi/admin | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/deposits | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/disputes | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/gamification | 32 | high | 0 | 30 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/promotions | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/readings | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/users | 31 | high | 0 | 29 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/68086b49-38df-4253-bb3b-51fbb3f82805 | 34 | high | 3 | 29 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/01efc087-0c26-4c10-ab45-5f2c53406c64 | 33 | high | 2 | 29 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/9caa0709-34bd-4cfd-a434-14056335f673 | 33 | high | 2 | 29 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 29 | high | 1 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 29 | high | 1 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 29 | high | 1 | 26 | 0 |
| logged-in-reader | mobile | auth-public | /vi | 34 | high | 4 | 27 | 0 |
| logged-in-reader | mobile | reading | /vi/reading | 29 | high | 1 | 26 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 31 | high | 1 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 32 | high | 2 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 31 | high | 1 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 33 | high | 3 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 29 | high | 1 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 29 | high | 1 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 29 | high | 1 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 35 | high | 1 | 32 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 29 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 29 | high | 1 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 29 | high | 1 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 29 | high | 1 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/e6e3bd1d-8c70-4a6d-afe6-f415b535ce9f | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/4b40f3c2-2175-4d5a-91d7-23b281a8512d | 33 | high | 2 | 29 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/e8ccde33-a90e-49f1-8b68-b0ec150b88c9 | 33 | high | 2 | 29 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 29 | high | 1 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 29 | high | 1 | 26 | 0 |

## High Slow Requests (>800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 2138 | 315 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1547 | 323 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1348 | 126 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1316 | 126 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1312 | 130 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1298 | 129 | static | https://www.tarotnow.xyz/_next/static/chunks/0kn_-kiqq.6et.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1282 | 78 | static | https://www.tarotnow.xyz/_next/static/chunks/0eqce2yjfryre.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1281 | 130 | static | https://www.tarotnow.xyz/_next/static/chunks/0d2b1s5.nlpim.js |
| logged-in-reader | mobile | auth-public | /vi | GET | 200 | 1272 | 83 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1268 | 70 | static | https://www.tarotnow.xyz/_next/static/chunks/0c4xf8mjx0gok.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1266 | 130 | static | https://www.tarotnow.xyz/_next/static/chunks/0ryoba0b~me9m.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1265 | 130 | static | https://www.tarotnow.xyz/_next/static/chunks/0xovby2vf96zv.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1259 | 63 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-reader | mobile | auth-public | /vi | GET | 200 | 1244 | 68 | static | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 1238 | 317 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1237 | 124 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | mobile | auth-public | /vi | GET | 200 | 1234 | 78 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1228 | 71 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | mobile | auth-public | /vi | GET | 200 | 1225 | 70 | static | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1195 | 65 | static | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | GET | 200 | 1191 | 314 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-reader | mobile | auth-public | /vi | GET | 200 | 1182 | 380 | html | https://www.tarotnow.xyz/vi |
| logged-in-reader | mobile | auth-public | /vi | GET | 200 | 1179 | 65 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 1174 | 88 | static | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 1164 | 98 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1154 | 95 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-out | mobile | auth-public | /vi | GET | 200 | 1147 | 630 | html | https://www.tarotnow.xyz/vi |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 1129 | 97 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | mobile | auth-public | /vi | GET | 200 | 1117 | 91 | static | https://www.tarotnow.xyz/_next/static/chunks/0d2b1s5.nlpim.js |
| logged-in-reader | mobile | auth-public | /vi | GET | 200 | 1112 | 77 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | mobile | auth-public | /vi | GET | 200 | 1109 | 90 | static | https://www.tarotnow.xyz/_next/static/chunks/0xovby2vf96zv.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1101 | 319 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 1100 | 96 | static | https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1060 | 325 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1059 | 128 | static | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1056 | 100 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 1051 | 96 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | mobile | auth-public | /vi | GET | 200 | 1043 | 72 | static | https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 1043 | 68 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 1035 | 74 | static | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 1024 | 544 | static | https://www.tarotnow.xyz/_next/static/chunks/0to-xfrh5ita1.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 1024 | 74 | static | https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| logged-in-reader | mobile | auth-public | /vi | GET | 200 | 1023 | 71 | static | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 1013 | 311 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| logged-in-reader | mobile | auth-public | /vi | GET | 200 | 1012 | 90 | static | https://www.tarotnow.xyz/_next/static/chunks/0h38g3weqde1h.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1010 | 80 | static | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-reader | mobile | auth-public | /vi | GET | 200 | 1000 | 70 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 997 | 91 | static | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | GET | 200 | 995 | 314 | html | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-reader | mobile | auth-public | /vi | GET | 200 | 995 | 89 | static | https://www.tarotnow.xyz/_next/static/chunks/0kn_-kiqq.6et.js |
| logged-in-reader | mobile | auth-public | /vi | GET | 200 | 991 | 83 | static | https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 990 | 984 | api | https://www.tarotnow.xyz/api/auth/session |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 990 | 80 | static | https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 989 | 130 | static | https://www.tarotnow.xyz/_next/static/chunks/0h38g3weqde1h.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 989 | 93 | static | https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 988 | 84 | static | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 988 | 130 | static | https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 975 | 96 | static | https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 974 | 97 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-reader | mobile | auth-public | /vi | GET | 200 | 971 | 89 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 971 | 97 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-reader | mobile | reading | /vi/reading | GET | 200 | 967 | 324 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | GET | 200 | 963 | 338 | html | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 954 | 96 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 943 | 129 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 939 | 129 | static | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 935 | 69 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | GET | 200 | 934 | 146 | static | https://www.tarotnow.xyz/_next/static/chunks/0kn_-kiqq.6et.js |
| logged-in-admin | mobile | reading | /vi/reading/history | GET | 200 | 930 | 324 | html | https://www.tarotnow.xyz/vi/reading/history |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 928 | 328 | html | https://www.tarotnow.xyz/vi/reading/history |
| logged-in-reader | mobile | auth-public | /vi | GET | 200 | 927 | 89 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | GET | 200 | 920 | 229 | static | https://www.tarotnow.xyz/_next/static/chunks/0ryoba0b~me9m.js |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 916 | 93 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | GET | 200 | 915 | 51 | telemetry | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| logged-in-admin | mobile | auth-public | /vi/legal/tos | GET | 200 | 912 | 328 | html | https://www.tarotnow.xyz/vi/legal/tos |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | GET | 200 | 908 | 285 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 903 | 131 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 902 | 95 | static | https://www.tarotnow.xyz/_next/static/chunks/0h38g3weqde1h.js |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 900 | 321 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 896 | 96 | static | https://www.tarotnow.xyz/_next/static/chunks/0xovby2vf96zv.js |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-admin | mobile | admin | /vi/admin/readings | GET | 200 | 800 | 333 | html | https://www.tarotnow.xyz/vi/admin/readings |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 799 | 325 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-admin | desktop | reading | /vi/reading | GET | 200 | 798 | 311 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 796 | 310 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1200&q=75 |
| logged-in-admin | mobile | reading | /vi/reading/history | GET | 200 | 796 | 81 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 794 | 311 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | GET | 200 | 791 | 371 | html | https://www.tarotnow.xyz/vi/admin/system-configs |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 790 | 327 | html | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 788 | 92 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 787 | 312 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-reader | desktop | auth-public | /vi | GET | 200 | 784 | 324 | html | https://www.tarotnow.xyz/vi |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 784 | 331 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 784 | 316 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 783 | 311 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-admin | desktop | reading | /vi/reading/session/d622091d-18e5-46d0-83d9-750a4e40ed88 | GET | 200 | 780 | 307 | html | https://www.tarotnow.xyz/vi/reading/session/d622091d-18e5-46d0-83d9-750a4e40ed88 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 779 | 149 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 778 | 341 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 778 | 317 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-out | desktop | auth-public | /vi/forgot-password | GET | 200 | 777 | 530 | html | https://www.tarotnow.xyz/vi/forgot-password |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 777 | 309 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 775 | 149 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 775 | 115 | static | https://www.tarotnow.xyz/_next/static/chunks/0xovby2vf96zv.js |
| logged-in-reader | mobile | reading | /vi/reading/session/e8ccde33-a90e-49f1-8b68-b0ec150b88c9 | GET | 200 | 775 | 317 | html | https://www.tarotnow.xyz/vi/reading/session/e8ccde33-a90e-49f1-8b68-b0ec150b88c9 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 773 | 317 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 772 | 319 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 771 | 316 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | desktop | reading | /vi/reading/session/8234c561-c559-4b9a-9661-e81e2b31cd67 | GET | 200 | 771 | 328 | html | https://www.tarotnow.xyz/vi/reading/session/8234c561-c559-4b9a-9661-e81e2b31cd67 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 771 | 101 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 769 | 86 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 769 | 310 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 769 | 299 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1200&q=75 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 768 | 311 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 768 | 96 | static | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 767 | 307 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | GET | 200 | 766 | 318 | html | https://www.tarotnow.xyz/vi/admin/reader-requests |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 765 | 314 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-reader | desktop | reading | /vi/reading/session/f895dbb3-9491-4e19-abd1-ba807082827f | GET | 200 | 764 | 335 | html | https://www.tarotnow.xyz/vi/reading/session/f895dbb3-9491-4e19-abd1-ba807082827f |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 763 | 111 | static | https://www.tarotnow.xyz/_next/static/chunks/0kn_-kiqq.6et.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 762 | 148 | static | https://www.tarotnow.xyz/_next/static/chunks/0kn_-kiqq.6et.js |
| logged-in-reader | mobile | auth-public | /vi | GET | 200 | 760 | 88 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 759 | 92 | static | https://www.tarotnow.xyz/_next/static/chunks/0d2b1s5.nlpim.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 757 | 326 | html | https://www.tarotnow.xyz/vi/profile/reader |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 755 | 313 | html | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | GET | 200 | 755 | 346 | html | https://www.tarotnow.xyz/vi/collection |
| logged-in-admin | mobile | admin | /vi/admin/gamification | GET | 200 | 751 | 326 | html | https://www.tarotnow.xyz/vi/admin/gamification |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 751 | 182 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 751 | 219 | static | https://www.tarotnow.xyz/_next/static/chunks/0eqce2yjfryre.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 749 | 114 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 749 | 111 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | GET | 200 | 748 | 311 | html | https://www.tarotnow.xyz/vi/collection |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 748 | 326 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 748 | 92 | static | https://www.tarotnow.xyz/_next/static/chunks/0xovby2vf96zv.js |
| logged-in-reader | desktop | reading | /vi/reading/history | GET | 200 | 746 | 338 | html | https://www.tarotnow.xyz/vi/reading/history |
| logged-in-reader | mobile | auth-public | /vi/legal/tos | GET | 200 | 746 | 323 | html | https://www.tarotnow.xyz/vi/legal/tos |
| logged-in-reader | desktop | reader-chat | /vi/chat | GET | 200 | 745 | 318 | html | https://www.tarotnow.xyz/vi/chat |
| logged-in-reader | desktop | auth-public | /vi | GET | 200 | 744 | 203 | static | https://www.tarotnow.xyz/_next/static/chunks/0kn_-kiqq.6et.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 741 | 148 | static | https://www.tarotnow.xyz/_next/static/chunks/0xovby2vf96zv.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 741 | 148 | static | https://www.tarotnow.xyz/_next/static/chunks/0d2b1s5.nlpim.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 739 | 326 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 738 | 313 | html | https://www.tarotnow.xyz/vi/profile/reader |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 737 | 326 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-admin | desktop | reading | /vi/reading/session/37a37db8-b409-43b3-823d-74327215621e | GET | 200 | 736 | 329 | html | https://www.tarotnow.xyz/vi/reading/session/37a37db8-b409-43b3-823d-74327215621e |
| logged-in-admin | mobile | auth-public | /vi/legal/tos | GET | 200 | 736 | 115 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 736 | 311 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 735 | 314 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 734 | 135 | static | https://www.tarotnow.xyz/_next/static/chunks/0kn_-kiqq.6et.js |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | GET | 200 | 732 | 324 | html | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 732 | 123 | static | https://www.tarotnow.xyz/_next/static/chunks/0h38g3weqde1h.js |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | GET | 200 | 732 | 117 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | reading | /vi/reading/session/01efc087-0c26-4c10-ab45-5f2c53406c64 | GET | 200 | 731 | 327 | html | https://www.tarotnow.xyz/vi/reading/session/01efc087-0c26-4c10-ab45-5f2c53406c64 |
| logged-in-admin | mobile | reading | /vi/reading/history | GET | 200 | 730 | 118 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 728 | 211 | telemetry | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 727 | 296 | html | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 727 | 330 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 727 | 148 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 726 | 326 | html | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| logged-in-admin | mobile | auth-public | /vi/legal/tos | GET | 200 | 725 | 141 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 724 | 364 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-out | desktop | auth-public | /vi/legal/tos | GET | 200 | 722 | 317 | html | https://www.tarotnow.xyz/vi/legal/tos |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | GET | 200 | 722 | 114 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |

## Pending Requests
| Scenario | Viewport | Route | URL |
| --- | --- | --- | --- |
| logged-in-reader | desktop | /vi/community | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/cdn-cgi/rum? |
| logged-in-reader | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |

## Coverage Notes
| Scenario | Viewport | Note |
| --- | --- | --- |
| logged-out | desktop | origin-discovery:/sitemap.xml:routes-22 |
| logged-out | desktop | origin-discovery:/robots.txt:routes-22 |
| logged-out | desktop | dynamic-routes: skipped for logged-out scenario. |
| logged-out | desktop | scenario-filter:logged-out-protected-routes-skipped=30 |
| logged-in-admin | desktop | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-admin | desktop | origin-discovery:/robots.txt:routes-22 |
| logged-in-admin | desktop | reading.init.daily_1: blocked (400). |
| logged-in-admin | desktop | reading.init.spread_3: created d622091d-18e5-46d0-83d9-750a4e40ed88. |
| logged-in-admin | desktop | reading.init.spread_5: created d25092e9-5583-4962-aea8-bb2f15a20746. |
| logged-in-admin | desktop | reading.init.spread_10: created 37a37db8-b409-43b3-823d-74327215621e. |
| logged-in-admin | desktop | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-admin | desktop | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-admin | desktop | reader-detail:ui-discovery-empty |
| logged-in-admin | desktop | chat-room-detail:ui-discovery-empty |
| logged-in-admin | desktop | reading-history-detail:ui-discovery-empty |
| logged-in-admin | desktop | community-posts:api-discovery-1 |
| logged-in-admin | desktop | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-admin | desktop | scenario-filter:admin-auth-entry-routes-skipped=5 |
| logged-in-reader | desktop | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-reader | desktop | origin-discovery:/robots.txt:routes-22 |
| logged-in-reader | desktop | reading.init.daily_1: blocked (400). |
| logged-in-reader | desktop | reading.init.spread_3: created f895dbb3-9491-4e19-abd1-ba807082827f. |
| logged-in-reader | desktop | reading.init.spread_5: created 8234c561-c559-4b9a-9661-e81e2b31cd67. |
| logged-in-reader | desktop | reading.init.spread_10: created fcf1ef30-e19e-497e-b9c8-5305083365fe. |
| logged-in-reader | desktop | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-reader | desktop | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-reader | desktop | reader-detail:ui-discovery-empty |
| logged-in-reader | desktop | chat-room-detail:ui-discovery-empty |
| logged-in-reader | desktop | reading-history-detail:ui-discovery-empty |
| logged-in-reader | desktop | community-posts:api-discovery-1 |
| logged-in-reader | desktop | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-reader | desktop | scenario-filter:reader-auth-entry-admin-routes-skipped=15 |
| logged-out | mobile | origin-discovery:/sitemap.xml:routes-22 |
| logged-out | mobile | origin-discovery:/robots.txt:routes-22 |
| logged-out | mobile | dynamic-routes: skipped for logged-out scenario. |
| logged-out | mobile | scenario-filter:logged-out-protected-routes-skipped=30 |
| logged-in-admin | mobile | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-admin | mobile | origin-discovery:/robots.txt:routes-22 |
| logged-in-admin | mobile | reading.init.daily_1: blocked (400). |
| logged-in-admin | mobile | reading.init.spread_3: created 68086b49-38df-4253-bb3b-51fbb3f82805. |
| logged-in-admin | mobile | reading.init.spread_5: created 01efc087-0c26-4c10-ab45-5f2c53406c64. |
| logged-in-admin | mobile | reading.init.spread_10: created 9caa0709-34bd-4cfd-a434-14056335f673. |
| logged-in-admin | mobile | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-admin | mobile | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-admin | mobile | reader-detail:ui-discovery-empty |
| logged-in-admin | mobile | chat-room-detail:ui-discovery-empty |
| logged-in-admin | mobile | reading-history-detail:ui-discovery-empty |
| logged-in-admin | mobile | community-posts:api-discovery-1 |
| logged-in-admin | mobile | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-admin | mobile | scenario-filter:admin-auth-entry-routes-skipped=5 |
| logged-in-reader | mobile | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-reader | mobile | origin-discovery:/robots.txt:routes-22 |
| logged-in-reader | mobile | reading.init.daily_1: blocked (400). |
| logged-in-reader | mobile | reading.init.spread_3: created e6e3bd1d-8c70-4a6d-afe6-f415b535ce9f. |
| logged-in-reader | mobile | reading.init.spread_5: created 4b40f3c2-2175-4d5a-91d7-23b281a8512d. |
| logged-in-reader | mobile | reading.init.spread_10: created e8ccde33-a90e-49f1-8b68-b0ec150b88c9. |
| logged-in-reader | mobile | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-reader | mobile | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-reader | mobile | reader-detail:ui-discovery-empty |
| logged-in-reader | mobile | chat-room-detail:ui-discovery-empty |
| logged-in-reader | mobile | reading-history-detail:ui-discovery-empty |
| logged-in-reader | mobile | community-posts:api-discovery-1 |
| logged-in-reader | mobile | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-reader | mobile | scenario-filter:reader-auth-entry-admin-routes-skipped=15 |

## Login Bootstrap Notes
### logged-in-admin / desktop
- Attempt 1: login bootstrap succeeded.

### logged-in-reader / desktop
- Attempt 1: login bootstrap succeeded.

### logged-in-admin / mobile
- Attempt 1: login bootstrap succeeded.

### logged-in-reader / mobile
- Attempt 1: login bootstrap succeeded.
