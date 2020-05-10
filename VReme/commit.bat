@echo off

git add --all
git commit -m "change %1"
git push

if not "%2" == "nopause" (
   pause
)