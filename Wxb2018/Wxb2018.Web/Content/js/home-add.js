$(function () {

    //下载路径
    var downloadPath = $('input[name="downloadPath"]').val();
    //上传路径
    var uploadPath = $('input[name="uploadPath"]').val();

    var uploader = WebUploader.create({
        // swf文件路径
        swf: '~/Content/plugins/webuploader/Uploader.swf',
        // 文件接收服务端。
        server: uploadPath,
        // 选择文件的按钮。可选。
        // 内部根据当前运行是创建，可能是input元素，也可能是flash.
        pick: {
            id: '#picker',
            multiple: false
        },
        //限制文件数量
        fileNumLimit: 5,
        //验证文件总大小是否超出限制, 超出则不允许加入队列(总共500M)
        fileSizeLimit: 524288000,
        //验证单个文件大小是否超出限制, 超出则不允许加入队列(单位B)(单个100M)
        fileSingleSizeLimit: 104857600,
        //自动上传文件
        auto: true,
        // 不压缩image, 默认如果是jpeg，文件上传前会压缩一把再上传！
        resize: false
    });

    //刪除已有文件
    $('.remove-item').on('click', function (e) {

        if (confirm('确认删除吗？')) {

            $(this).parent().remove();
        }

        e.stopPropagation();
    });

    //当文件被加入队列之前触发，此事件的handler返回值为false，则此文件不会被添加进入队列。
    uploader.on('beforeFileQueued', function (file) {

        if (file.size == 0) {
            alert('大小为0的文件不能上传');
            return false;
        }

        var fileSize = file && file.size ? Math.round(file.size / (1 * 1024 * 1024)) : 0;
        if (fileSize > 100) {
            alert('单个文件大小不能超过100M');
            return false;
        }

        var totalFileSize = 0;
        $('.uploader-list .item .file-name').each(function (i, val) {
            totalFileSize += $(val).data('size');
        });

        totalFileSize = Math.round(totalFileSize / (1 * 1024 * 1024));
        if (totalFileSize > 500) {
            alert('文件总大小不能超过500M');
            return false;
        }

        var fileNum = $('.uploader-list .item').length;
        if (fileNum >= 5) {
            alert('文件数量不能超过5个');
            return false;
        }

    });

    // 当有文件被添加进队列的时候
    uploader.on('fileQueued', function (file) {

        var temp = '';
        if (file.size < 1024) {

            temp = file.size + 'B';
        }
        else if (file.size >= 1024 && file.size < 1024 * 1024) {

            temp = (file.size / 1024).toFixed(2) + 'KB';
        } else if (file.size >= 1024 * 1024) {

            temp = (file.size / (1024 * 1024)).toFixed(2) + 'M';
        }

        var $list = $('#thelist');
        $list.append('<div id="' + file.id + '" class="item">' +
                         '<a class="file-name" data-path="" data-name="' + file.name + '" data-size="' + (file.size) + '" target="_self" style="display:inline-block;margin-right:5px;" >' + file.name + '</a>' +
                         '<i class="file-size" style="margin-right:10px;">' + temp + '</i>' +
                         '<a class="remove-item iconfont icon-delete" href="javascript:void(0);" style="display:inline-block;text-decoration: none;" title="删除"></a>' +
        //'<h4 class="info">' + file.name + '</h4>' +
        //'<p class="state">等待上传...</p>' + 
                     '</div>');

        //刪除
        $list.on('click', '.remove-item', function (e) {

            //注意这里事件委托
            if ($(this).parent().attr('id') != file.id) return false;

            if (confirm('确认删除吗？')) {

                uploader.removeFile(file);
                $(this).parent().remove();
            }

            e.stopPropagation();
            return false;
        });

    });

    // 文件上传过程中创建进度条实时显示。
    uploader.on('uploadProgress', function (file, percentage) {

        var $li = $('#' + file.id),
            $percent = $li.find('.progress .progress-bar');

        // 避免重复创建
        if (!$percent.length) {
            $percent = $('<div class="progress progress-striped active">' +
                            '<div class="progress-bar" role="progressbar" style="width: 0%">' +
                            '</div>' +
                         '</div>').appendTo($li).find('.progress-bar');
        }

        //$li.find('p.state').text('上传中');
        $percent.css('width', percentage * 100 + '%');
    });

    //文件上传失败会派送uploadError事件，成功则派送uploadSuccess事件。
    //不管成功或者失败，在文件上传完后都会触发uploadComplete事件。
    //上传成功
    uploader.on('uploadSuccess', function (file, response) {

        var href = downloadPath + '?filePath=' + response.Data.ViewUrl + '&fileName=' + file.name;
        var path = response.Data.ViewUrl;
        $('#' + file.id + ' a.file-name').attr('href', href);
        $('#' + file.id + ' a.file-name').data('path', path);

        //$('#' + file.id).find('p.state').text('已上传');
        //uploader.removeFile(file);
    });

    //上传失败
    uploader.on('uploadError', function (file, reason) {
        $('#' + file.id).find('p.state').text('上传出错');
    });

    //上传完成
    uploader.on('uploadComplete', function (file) {

        $('#' + file.id).find('.progress').fadeOut();
    });

    //文件大小转换
    function fileSizeExchange(size) {
        if (!size) return 0;
    }

    //开始上传
    $('#ctlBtn').on('click', function () {

        uploader.upload();
    });

    //其他业务代码
    //选择人员
    $('.choose-user').on('click', function () {

        var tempHtml = '<div class="user-show">' +
                            '<div class="show-title"><span class="title-text">请选择角色</span></div>' +
                            '<div class="show-content"></div>' +
                       '</div>';

        var options = {
            title: false,
            type: 1,
            skin: 'user-data',
            area: ['740px', '300px'],
            closeBtn: false,
            shade: 0.3,
            move: false,
            content: tempHtml,
            btn: ['保 存', '取 消'],
            yes: function (index, layero) {

                var roleHtml = '', userHtml = '';
                $('.show-content .item-role input:checked').each(function (i, val) {

                    roleHtml += '<span class="con-item" data-userid="' + $(val).val() + '" >' + $(val).siblings('label').text() + '</span>';
                });

                $('.show-content .item-user input:checked').each(function (i, val) {

                    userHtml += '<span class="con-item" data-userid="' + $(val).val() + '" >' + $(val).siblings('label').text() + '</span>';
                });

                if (roleHtml != '') {
                    $('.list-con .choosed-roles').html(roleHtml);
                } else {
                    $('.list-con .choosed-roles').html('');
                }

                if (userHtml != '') {
                    $('.list-con .choosed-users').html(userHtml);
                } else {
                    $('.list-con .choosed-users').html('');
                }

                layer.close(index);
            },
            btn2: function (index, layero) {
                layer.close(index);
            }
        };

        $.ajax({
            url: $('input[name="getUserListUrl"]').val(),
            dataType: 'json',
            type: 'get',
            success: function (data) {

                if (data.Code < 0) {
                    alert(data.Msg);
                    return;
                }

                var result = data.Data;
                var dataHtml = '', subItem = '';
                for (var i = 1; i <= result.length; i++) {

                    subItem += '<span class="item-wrap">' +
                                    '<input type="checkbox"  value="' + result[i - 1].ID + '" name="chooseUser" id="user-id' + result[i - 1].ID + '" />' +
                                    '<label class="wrap-text ellipsis" for="user-id' + result[i - 1].ID + '" title="' + result[i - 1].Name + '">' + result[i - 1].Name + '</label>' +
                               '</span>';

                    if (i % 10 == 0) {
                        dataHtml += '<div class="content-item item-user">' + subItem + '</div>';
                        subItem = '';
                    }

                    if (i == result.length && i % 10 != 0) {
                        dataHtml += '<div class="content-item item-user">' + subItem + '</div>';
                    }
                }

                options.success = function (layero, index) {
                    //按钮居中
                    layero.find('.layui-layer-btn').css('text-align', 'center');

                    var content = '<div class="content-item item-role">' +
                                     '<span class="item-wrap"><input type="checkbox" name="chiefEditor" value="1" id="chiefEditor" /><label for="chiefEditor">主编</label></span>' +
                                     '<span class="item-wrap"><input type="checkbox" name="edited" value="2" id="edited" /><label for="edited">责编</label></span>' +
                                     '<span class="item-wrap"><input type="checkbox" name="editor" value="3" id="editor" /><label for="editor">编辑</label></span>' +
                                   '</div>';

                    $('.show-content').html(content);
                    $('.show-content').append('<div class="show-title"><span class="title-text">请选择人员</span></div>');
                    $('.show-content').append(dataHtml);


                    var roleIds = '';
                    $('.choosed-roles .con-item').each(function (i, val) {
                        roleIds += $(val).data('userid') + ',';
                    });

                    $('.item-role .item-wrap input').each(function (i, val) {

                        if (roleIds && roleIds.indexOf($(val).val()) != -1)
                            $(val).prop('checked', true);
                    });

                    var userIds = '';
                    $('.choosed-users .con-item').each(function (i, val) {
                        userIds += $(val).data('userid') + ',';
                    });

                    $('.item-user .item-wrap input').each(function (i, val) {

                        if (userIds && userIds.indexOf($(val).val()) != -1)
                            $(val).prop('checked', true);
                    });

                };

                layer.open(options);
            }
        });
    });

    //获取用户列表
    function getUserList() {

        $.ajax({
            url: $('input[name="getUserListUrl"]').val(),
            dataType: 'json',
            type: 'get',
            success: function (data) {

                if (data.Code < 0) {
                    alert(data.Msg);
                    return;
                }

                var result = data.Data;
                var dataHtml = '', subItem = '';
                for (var i = 1; i <= result.length; i++) {

                    subItem += '<span class="item-wrap">' +
                                    '<input type="checkbox"  value="' + result[i - 1].ID + '" name="chooseUser" id="user-id' + result[i - 1].ID + '" />' +
                                    '<label for="user-id' + result[i - 1].ID + '">' + result[i - 1].Name + '</label>' +
                               '</span>';

                    if (i % 10 == 0) {
                        dataHtml += '<div class="content-item">' + subItem + '</div>';
                        subItem = '';
                    }

                    if (i == result.length && i % 10 != 0) {
                        dataHtml += '<div class="content-item">' + subItem + '</div>';
                    }
                }

                $('.show-content').html(dataHtml);
            }
        });
    }

    //返回
    $('.goBack').on('click', function (e) {
        window.history.back();
    });

    //防止二次提交
    var isPostBack = true;
    //保存
    $('#noticeSave').on('click', function () {

        if (!isPostBack) return;
        isPostBack = false;

        var noticeId = $('input[name="noticeId"]').val(),
            title = $('input[name="noticeTitle"]').val(),
         content = $('textarea[name="noticeContent"]').val();

        if ($.trim(title) == '') {

            alert('通知标题不能为空！');
            isPostBack = true;
            return false;
        }

        if (title.replace(/\s/g, '').length > 100) {
            alert('通知标题不能超过100个字符！');
            isPostBack = true;
            return false;
        }

        if ($.trim(content) == '') {

            alert('通知内容不能为空！');
            isPostBack = true;
            return false;
        }

        var roleSelected = $('.list-con .choosed-roles .con-item'),
           userSelected = $('.list-con .choosed-users .con-item'),
           selectedFile = $('#thelist .file-name'),
           choosedUserIds = '',
           choosedRoleIds = '',
           chooseFileUrls = '';

        //选中的角色
        roleSelected.each(function (i, val) {

            if (i == roleSelected.length - 1)
                choosedRoleIds += $(val).data('userid');
            else
                choosedRoleIds += $(val).data('userid') + ',';
        });

        //选中的用户
        userSelected.each(function (i, val) {

            if (i == userSelected.length - 1)
                choosedUserIds += $(val).data('userid');
            else
                choosedUserIds += $(val).data('userid') + ',';
        });

        selectedFile.each(function (i, val) {

            if (i == selectedFile.length - 1)
                chooseFileUrls += $(val).data('path') + '>' + $(val).data('name') + '>' + $(val).data('size');
            else
                chooseFileUrls += $(val).data('path') + '>' + $(val).data('name') + '>' + $(val).data('size') + '|';
        });

        var saveUrl = $('input[name="saveNoticeUrl"]').val();
        var noticeParams = {
            ID: noticeId,
            Title: title.replace(/\s/g, ''),
            Content: content,
            ChoosedUserIds: choosedUserIds,
            ChooseRoles: choosedRoleIds,
            AttachmentUrl: encodeURIComponent(chooseFileUrls)
        };

        $.post(saveUrl, noticeParams, function (data) {

            isPostBack = true;
            if (data.Code == 200) {

                //刷新界面
                location.href = '/';

            } else {
                alert(data.Msg);
            }
        });
    });
});