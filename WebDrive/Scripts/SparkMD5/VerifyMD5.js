
            var blobSlice = File.prototype.slice || File.prototype.mozSlice || File.prototype.webkitSlice,
                log = document.getElementById("log"),
                input = document.getElementById("file_upload"),
                running = false;

            function registerLog(str, className) {
                var elem = document.createElement("div");
                elem.innerHTML = str;
                elem.className = "alert-message" + (className ? " "  + className : "");
                log.appendChild(elem);
            }

            if (!("FileReader" in window) || !("File" in window) || !blobSlice) {
                registerLog("<p><strong>Your browser does not support the FileAPI or slicing of files.</strong></p>", "error");
            }
            else {
                var ua = navigator.userAgent.toLowerCase();

                if (/chrome/.test(ua)) {
                    if (location.protocol === "file:") {
                        registerLog("<p><strong>This example will only work in chrome (in file:// protocol) if you start it up with -allow-file-access-from-files argument.</strong><br/>This is a security measure introduced in chrome, please <a target=\"_blank\" href=\"http://code.google.com/p/chromium/issues/detail?id=60889\">see</a>.</p>");
                    }
                }
                else if (/firefox/.test(ua)) {
                    var firebugEnabled = !!(window.console && (window.console.firebug || (console.exception && console.table)));
                    if (firebugEnabled) registerLog("<p><strong>It seems you got firebug enabled.</strong><br/>Firebug slows down this script by a great margin and causes high memory/cpu usage, please disable it and use the built in web console instead.</p>");
                }
                else if (/opera/.test(ua)) {
                    registerLog("<p><strong>If you got DragonFly open please consider closing it as it slows down the test by a great margin.</strong></p>");
                }

                function doIncrementalTest(upload_id) {
                    var queueId = '#uploadifive-' + upload_id + '-queue';
                    window.ququeID = queueId;
                    var files = $(queueId).find('.uploadifive-queue-item').not('.error, .complete').data('file');

                    if (running) return;
                    /*if (input.files.length == 0) {
                        registerLog("<strong>Please select a file.</strong><br/>");
                        return;
                    }*/

                    var blobSlice = File.prototype.slice || File.prototype.mozSlice || File.prototype.webkitSlice,
                        file = files,
                        chunkSize = 2097152,                           // read in chunks of 2MB
                        chunks = Math.ceil(file.size / chunkSize),
                        currentChunk = 0,
                        spark = new SparkMD5.ArrayBuffer(),
                        time,
                        uniqueId = "chunk_" + (new Date().getTime()),
                        chunkId = null,

                    frOnload = function(e) {

                        if (currentChunk == 0) registerLog("Read chunk number <strong id=\"" + uniqueId + "\">" + (currentChunk + 1) + "</strong> of <strong>" + chunks + "</strong><br/>", "info");
                        else {
                            if (chunkId === null) chunkId = document.getElementById(uniqueId);
                            chunkId.innerHTML = currentChunk + 1;
                        }

                        spark.append(e.target.result);                 // append array buffer
                        currentChunk += 1;

                        if (currentChunk < chunks) {
                            loadNext();
                        }
                        else {
                            running = false;
                            window.MD5string = spark.end();
                            registerLog("<strong>Finished loading!</strong><br/>", "success");
                            registerLog("<strong>Computed hash:</strong> " + window.MD5string + "<br/>", "success"); // compute hash
                            registerLog("<strong>Total time:</strong> " + (new Date().getTime() - time) + "ms<br/>", "success");
                            var url = '/RealFile/IsRepeatMD5';
                            var postUrl = '/RealFile/PostMD5';
                            var data = {
                                MD5: window.MD5string,
                            };
                            $.get(url, data ,function(response){
                                if(!response.IsRepeat){
                                    $('#file_upload').uploadifive('upload');
                                }else{
                                    $(window.ququeID).find('.uploadifive-queue-item').not('.error, .complete').data('file').queueItem.find('.progress-bar').css('width', 100 + '%');
                                    $(window.ququeID).find('.uploadifive-queue-item').not('.error, .complete').data('file').queueItem.find('.fileinfo').html(' - ' + 100 + '%');
                                    var fileInfo = $(window.ququeID).find('.uploadifive-queue-item').not('.error, .complete').data('file');
                                    var splitName = fileInfo.name.split('.');
                                    var fileName = "";
                                    var fileType = "";
                                    for (var i = 0; i < splitName.length - 1; i++) {
                                        fileName += splitName[i];
                                    }
                                    fileType = splitName[splitName.length - 1];
                                    var url = '/UserFile/AddExistFile';
                                    var data = {
                                        fileName : fileName, 
                                        fileType : fileType, 
                                        parentID: opener.currentDir.currentDirID,
                                        realFileID : response.RealFileID
                                    };
                                    if (fileName && fileType) {
                                        $.get(url, data, function (response) {
                                            if (response.success) {
                                                opener.fileName = response.FileName;
                                                opener.userFileID = response.UserFileID;
                                                window.close();
                                            }
                                        });
                                    } else {
                                        alert("File name and type is needed!");
                                        window.close();
                                    }
                                }
                            })
                            
                        }
                    },

                   frOnerror = function() {
                        running = false;
                        registerLog("<strong>Oops, something went wrong.</strong>", "error");
                   },

                    loadNext = function() {
                        var fileReader = new FileReader();
                        fileReader.onload = frOnload;
                        fileReader.onerror = frOnerror;

                        var start = currentChunk * chunkSize,
                            end = ((start + chunkSize) >= file.size) ? file.size : start + chunkSize;

                        fileReader.readAsArrayBuffer(blobSlice.call(file, start, end));
                    };

                    running = true;
                    registerLog("<p></p><strong>Starting incremental test (" + file.name + ")</strong><br/>", "info");
                    time = new Date().getTime();
                    loadNext();
                }

                function doNormalTest() {

                    if (running) return;
                    if (input.files.length == 0) {
                        registerLog("<strong>Please select a file.</strong><br/>");
                        return;
                    }

                    var fileReader = new FileReader(),
                        file = input.files[0],
                        time;

                    fileReader.onload = function(e) {

                        running = false;

                        if (file.size != e.target.result.length) {
                            registerLog("<strong>ERROR:</strong> Browser reported success but could not read the file until the end.<br/>", "error");
                        }
                        else {
                            registerLog("<strong>Finished loading!</strong><br/>", "success");
                            registerLog("<strong>Computed hash:</strong> " + SparkMD5.ArrayBuffer.hash(e.target.result) + "<br/>", "success"); // compute hash
                            registerLog("<strong>Total time:</strong> " + (new Date().getTime() - time) + "ms<br/>", "success");
                        }
                    };

                    fileReader.onerror = function(e) {
                        running = false;
                        registerLog("<strong>ERROR:</strong> FileReader onerror was triggered, maybe the browser aborted due to high memory usage.<br/>", "error");
                    };

                    running = true;
                    registerLog("<strong>Starting normal test (" + file.name + ")</strong><br/>", "info");
                    time = new Date().getTime();
                    fileReader.readAsArrayBuffer(file);
                }

                function clearLog() {
                    log.getChildren().destroy();
                }
            }

            //document.getElementById("normal").addEventListener("click", doNormalTest);
            //document.getElementById("incremental").addEventListener("click", doIncrementalTest);
            //document.getElementById("clear").addEventListener("click", clearLog);