git config --global user.name "Daniel Georgiev"
git config --global user.email daniel.georgiev95@gmail.com
git config --global credential.helper store
git config --global push.default simple

git submodule update --init --recursive
git submodule foreach git checkout master
