# SSH Key Setup Guide

## 1. Generate SSH Keys

1. **Create the `.ssh` directory** (if it doesn't exist):

   ```cmd
   mkdir C:\Users\lucas.hartman\.ssh
   ```

2. **Generate SSH key for your *work account*:**

   ```cmd
   ssh-keygen -t ed25519 -C "lucas.hartman@ssl.nl"
   ```

   * When prompted for file path, enter:

     ```cmd
     C:\Users\lucas.hartman\.ssh\id_ed25519_work
     ```
   * Enter passphrase:

     ```
     *****
     ```

3. **Generate SSH key for your *personal account*:**

   ```cmd
   ssh-keygen -t ed25519 -C "lucas.hartman@proton.me"
   ```

   * When prompted for file path, enter:

     ```cmd
     C:\Users\lucas.hartman\.ssh\id_ed25519_personal
     ```
   * Enter passphrase:

     ```
     *****
     ```

---


## 2. Add SSH Keys to the SSH Agent

### Step 1: **Enable the SSH agent (run as Admin)**

```powershell
Set-Service -Name ssh-agent -StartupType Manual
Start-Service ssh-agent
```


---


### 3: **Set correct permissions for the SSH key files**

1. **For Work Key:**

   * Run in terminal (as Admin):

     ```powershell
     icacls "C:\Users\lucas.hartman\.ssh\id_ed25519_work" /inheritance:r /grant:r "$($env:USERNAME):R"
     ```

   * If you get an "Access Denied" error:

     * Navigate to the file manually.
     * Right-click > Properties > Security > Advanced.
     * Ensure only `lucas.hartman` has read/write permissions.

   * Add key to SSH agent:

     ```powershell
     ssh-add C:\Users\lucas.hartman\.ssh\id_ed25519_work
     ```

     * Enter passphrase:

       ```
       *****
       ```

2. **For Personal Key:**

   * Set permissions:

     ```powershell
     icacls "C:\Users\lucas.hartman\.ssh\id_ed25519_personal" /inheritance:r /grant:r "$($env:USERNAME):R"
     ```

   * Add key to SSH agent:

     ```powershell
     ssh-add C:\Users\lucas.hartman\.ssh\id_ed25519_personal
     ```

     * Enter passphrase:

       ```
       *****
       ```

---

## 3: Add Your Public Key to GitHub

1. **Open the public key in Notepad:**

    * In PowerShell or Command Prompt, run:
    ```
    notepad C:\Users\lucas.hartman\.ssh\id_ed25519_work.pub
    ```
    * This will open a file that starts with something like:
    * Select all the text and copy it.

2. **Go to your GitHub (Work Account):**

    * Visit [github](https://github.com)
    * Make sure you're logged into your work GitHub account
    * In the top right, click your profile picture → Settings
    * In the sidebar, click "SSH and GPG keys"
    * Click the green "New SSH key" button

3. **Paste and Save:**

    * Title: Work Laptop or anything you like
    * Key type: Leave as Authentication Key
    * Key: Paste the public key you copied from Notepad
    * Click "Add SSH key".

4. **Repeat for Personal**

---

## 4: Configure SSH Config File
* Edit (or create) your SSH config:
```Terminal
notepad C:\Users\lucas.hartman\.ssh\config
```
* Add this:
``` Terminal
# Personal GitHub
Host github-personal
  HostName github.com
  User git
  IdentityFile ~/.ssh/id_ed25519_personal

# Work GitHub
Host github-work
  HostName github.com
  User git
  IdentityFile ~/.ssh/id_ed25519_work
```
* Make sure the config file is not txt format:
```Terminal
Rename-Item -Path config.txt -NewName config
```
* Set config
```Terminal
$env:GIT_SSH_COMMAND="ssh -F C:/Users/lucas.hartman/.ssh/config"
```

---

## 5: Clone project
* Create a Project/Personal directory
* Clone you personal project like this:
```
# Project/Personal
git clone git@github-personal:LucasHartman/Microservices_Example.git

# Project/Work
git clone git@github-work:Ship-Spares-Logistics/SslWebServices.git
```

## 6. Github
* You can verify that your SSH configuration is working correctly by running:
```
ssh -T git@github-personal
ssh -T git@github-work
```
* Check current remote URL: `git remote -v`
* Set remote URL:
```
git remote set-url origin git@github-personal:LucasHartman/Webservices.git
git remote set-url origin git@github-work:Ship-Spares-Logistics/Webservices.git
```
* Remote remote: `git remote remove origin`
* Push master: `git push -u origin master`
* Test SSH connection: `ssh -T git@github-personal`
* Run SSH Agent:
```
Get-Service ssh-agent
Start-Service ssh-agent
```
* Add key again: `ssh-add C:\Users\lucas.hartman\.ssh\id_ed25519_personal`
* Push: `git push --set-upstream origin main`

# 7. Change SSH Key Passphrases
```
ssh-keygen -p -f C:\Users\lucas.hartman\.ssh\id_ed25519_work
ssh-keygen -p -f C:\Users\lucas.hartman\.ssh\id_ed25519_personal
```