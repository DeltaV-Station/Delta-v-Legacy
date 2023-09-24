#!/usr/bin/perl
use 5.010;
use strict;
use warnings;
use File::Find;

use Cwd qw(getcwd);
#change this to find fields..
my @tagStr = ('solids', 'reagents');
#change this to find file types.
my $fileSubstr = 'meal_recipes.yml';
#change this to change the source directory being searched in.
my $fldrName = "Prototypes";
my $fileName = 'wiki.txt';
 
 
open(FH, '>', $fileName) or die $!;
chdir "../";
chdir "Resources";
my $root = getcwd();
find(
{
    wanted => \&findfiles,
},
$fldrName
);
my @files = ();
sub findfiles
{
    my $file = $_;
    #change this string to desired type.
    if( index($file, $fileSubstr) != -1 ){
        my $fileRef = $root . "\\" . "$File::Find::name";
        $fileRef =~ s/\\/\//g; # Tries to make the filename work well on Windows. Probably doesn't work worth a damn.
        print FH "\n"; 
        open (RES, $fileRef) or die "could not open $file\n";
        my $tagIdx = -1;
        my $seekIdx = -1;
        while(<RES>){
            $seekIdx++;
            #finds names.
            my $namestr = 'name';
            my $nameIdx = index( "$_", $namestr );
            if ( $nameIdx != -1 ) {
                my $name = "$_";
                s/([\w']+)/\u\L$1/g; #Capitalizes each word in the name of the food, for appearance's sake. 
                s/\&/And/; #Replaces the & character with 'And' because special characters DO NOT BELONG IN NAMES.
                print FH;
            }

            #finds id for system.
            my $idStr = 'id';
            my $check = $_;
            $check =~ s/^\s+//; #Removes leading whitespace. 
            $check = index($check, "#");
            my $idIdx = index( "$_", $idStr );
            if ( $idIdx != -1 ) {
                my $id = "$_";
                if($check != 0 && $nameIdx == -1){
                    print FH; 
                }
            }

            #finds ingredients.
            foreach my $interestStr (@tagStr) {
                my $isIdx = index( "$_", $interestStr );
                if( $tagIdx == -1){
                    $tagIdx = $isIdx;
                }
            }
            if ( $tagIdx != -1 && $idIdx == -1 && $nameIdx == -1 ){
                if( "$_" eq "\n" ){
                    $tagIdx = -1;
                }
                #deal with comments
                my $commentsCheck = index("$_", "#"); 
                if( ($commentsCheck > 8 ||  $commentsCheck == -1) ) {
                    my $output = "$_";
                    if( $commentsCheck > 8 ){
                        $output =~ s/\#.*//; #removes # character. I have no idea how \$ evaluates to that. Thanks Perl.
                    }
                    print FH $output;
                }
            }
        }
        push(@files, "$File::Find::name");
    }
}
close(FH);
